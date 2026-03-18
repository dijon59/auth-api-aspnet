using System.ComponentModel.DataAnnotations;


namespace BackendApi.DTOs;


public class RegisterHttpRequest
{   [Required(ErrorMessage = "First name is required")]
    public string FirstName {get; set;} = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName {get; set;} = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    public string Email {get; set;} = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginHTTPRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token {get; set;} = string.Empty;
    public string Username {get; set;} = null!;
}

public class UserResponse
{
    // public int Id {get; set;}
    public string FirstName {get; set;} = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
