using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GestionProductos.Common;

public class LoginSuccessMessage:ValueChangedMessage<bool>
{
    public LoginSuccessMessage(bool value) : base(value) { }
}
