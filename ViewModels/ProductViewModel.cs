using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionProductos.Models;
using GestionProductos.Services;
using Microsoft.Extensions.Logging;

namespace GestionProductos.ViewModels;

public class ProductViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductViewModel> _logger;
    private CancellationTokenSource? _reloadCts;

    public ProductViewModel(IProductService productService, ILogger<ProductViewModel> logger)
    {
        _productService = productService;
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

    public IAsyncRelayCommand LoadCommand { get; }
    public IRelayCommand ClearFiltersCommand { get; }

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
}