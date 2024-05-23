using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductAPI.Data;
public class RegisterModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password é obrigatório")]
    public string? Password { get; set; }

    [EmailAddress] 
    [Required(ErrorMessage = "Email é obrigatório")]
    public string? Email { get; set; }
}
