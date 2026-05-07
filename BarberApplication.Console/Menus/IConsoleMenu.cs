namespace BarberApplication.Console.Menus;

/// <summary>
/// Interface para os menus do console da aplicação.
/// </summary>
public interface IConsoleMenu
{
    /// <summary>
    /// Exibe o menu e processa as interações do usuário.
    /// </summary>
    Task ExibirAsync();
}
