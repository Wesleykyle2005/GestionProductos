using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionProductos.Models;
using GestionProductos.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace GestionProductos.ViewModels;

public class ProductViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IOptionService _optionService;
    private readonly ILogger<ProductViewModel> _logger;
    private CancellationTokenSource? _reloadCts;

    public ProductViewModel(IProductService productService, IOptionService optionService, ILogger<ProductViewModel> logger)
    {
        _productService = productService;
        _optionService = optionService;
        _logger = logger;

        StatusList = new[]
        {
            new KeyValuePair<string, bool?>("Todos",    null),
            new KeyValuePair<string, bool?>("Activos",  true),
            new KeyValuePair<string, bool?>("Inactivos",false),
        };
        _selectedStatus = StatusList.First();

        LoadCommand = new AsyncRelayCommand(LoadAsync);
        ClearFiltersCommand = new RelayCommand(ClearFilters);
        ShowDetailsCommand = new RelayCommand<Producto>(ShowProductDetails);
        HideDetailsCommand = new RelayCommand(HideProductDetails);

        AddNewOptionCommand = new RelayCommand(PrepareForNewOption, () => SelectedProduct != null);
        EditOptionCommand = new RelayCommand<Opcion>(PrepareForEditOption);
        SaveOptionCommand = new AsyncRelayCommand(SaveOptionAsync, () => !string.IsNullOrWhiteSpace(OptionName));
        CancelEditCommand = new RelayCommand(CancelEdit);
        DeleteOptionCommand = new AsyncRelayCommand<Opcion>(DeleteOptionAsync);

        _ = LoadCommand.ExecuteAsync(null);
    }

    public ObservableCollection<Producto> Products { get; } = new();

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ScheduleReload();
            }
        }
    }
    private string _searchText = string.Empty;

    public IEnumerable<KeyValuePair<string, bool?>> StatusList { get; }

    public KeyValuePair<string, bool?> SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (SetProperty(ref _selectedStatus, value))
            {
                _ = LoadAsync();
            }
        }
    }
    private KeyValuePair<string, bool?> _selectedStatus;

    public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
    private bool _isBusy;

    public string? Error { get => _error; set => SetProperty(ref _error, value); }
    private string? _error;

    private Producto? _selectedProduct;
    public Producto? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (SetProperty(ref _selectedProduct, value))
            {
                AddNewOptionCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _isPopupVisible;
    public bool IsPopupVisible
    {
        get => _isPopupVisible;
        set => SetProperty(ref _isPopupVisible, value);
    }

    private Opcion? _editingOption;
    public Opcion? EditingOption { get => _editingOption; private set => SetProperty(ref _editingOption, value); }

    private string _optionName = string.Empty;
    public string OptionName
    {
        get => _optionName;
        set
        {
            if (SetProperty(ref _optionName, value))
            {
                SaveOptionCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _isEditingFormVisible;
    public bool IsEditingFormVisible { get => _isEditingFormVisible; set => SetProperty(ref _isEditingFormVisible, value); }

    public IAsyncRelayCommand LoadCommand { get; }
    public IRelayCommand ClearFiltersCommand { get; }
    public IRelayCommand<Producto> ShowDetailsCommand { get; }
    public IRelayCommand HideDetailsCommand { get; }
    public IRelayCommand AddNewOptionCommand { get; }
    public IRelayCommand<Opcion> EditOptionCommand { get; }
    public IAsyncRelayCommand SaveOptionCommand { get; }
    public IRelayCommand CancelEditCommand { get; }
    public IAsyncRelayCommand<Opcion> DeleteOptionCommand { get; }


    private void ClearFilters()
    {
        _searchText = string.Empty;
        _selectedStatus = StatusList.First();
        OnPropertyChanged(nameof(SearchText));
        OnPropertyChanged(nameof(SelectedStatus));
        _ = LoadAsync();
    }

    private void ScheduleReload()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        _ = ReloadAfterDelayAsync(_reloadCts.Token);
    }

    private async Task ReloadAfterDelayAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(300, token);
            await LoadAsync();
        }
        catch (TaskCanceledException)
        {
        }
    }

    private async Task LoadAsync()
    {
        if (IsBusy) return;
        Error = null;
        IsBusy = true;

        try
        {
            var nameFilter = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim();
            var statusFilter = SelectedStatus.Value;

            var items = await Task.Run(() => _productService.SearchAsync(nameFilter, statusFilter));

            Products.Clear();
            foreach (var p in items)
            {
                Products.Add(p);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cargando productos");
            Error = "No se pudieron cargar los productos. Inténtalo más tarde.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ShowProductDetails(Producto? product)
    {
        if (product == null) return;
        SelectedProduct = product;
        IsPopupVisible = true;
    }

    private void HideProductDetails()
    {
        IsPopupVisible = false;
        CancelEdit();
        SelectedProduct = null;
    }

    private void PrepareForNewOption()
    {
        EditingOption = new Opcion { Nombre = "", CodigoProducto = SelectedProduct!.Codigo };
        OptionName = "";
        IsEditingFormVisible = true;
    }

    private void PrepareForEditOption(Opcion? option)
    {
        if (option == null) return;
        EditingOption = option;
        OptionName = option.Nombre;
        IsEditingFormVisible = true;
    }

    private async Task SaveOptionAsync()
    {
        if (EditingOption == null || SelectedProduct == null) return;

        EditingOption.Nombre = OptionName;

        try
        {
            if (EditingOption.IdOpcion == 0)
            {
                var newOption = await _optionService.AddOptionAsync(EditingOption);
                SelectedProduct.Opciones.Add(newOption);
            }
            else
            {
                var updatedOption = await _optionService.UpdateOptionAsync(EditingOption);
                
                var oldOption = SelectedProduct.Opciones.FirstOrDefault(o => o.IdOpcion == updatedOption.IdOpcion);
                if (oldOption != null)
                {
                    SelectedProduct.Opciones.Remove(oldOption);
                    SelectedProduct.Opciones.Add(updatedOption);
                }
            }
            CancelEdit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar la opción.");
            Error = "No se guardar la opción.";
        }
    }

    private void CancelEdit()
    {
        IsEditingFormVisible = false;
        EditingOption = null;
        OptionName = "";
    }

    private async Task DeleteOptionAsync(Opcion? option)
    {
        if (option == null || SelectedProduct == null) return;

        try
        {
            await _optionService.DeleteOptionAsync(option.IdOpcion);
            SelectedProduct.Opciones.Remove(option);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la opción.");
            Error = "No se pudo eliminar la opción. Es posible que esté en uso.";
        }
    }
}