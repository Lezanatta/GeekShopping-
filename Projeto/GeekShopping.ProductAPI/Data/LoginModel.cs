using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductAPI.Data;
public class LoginModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string? UserName { get; set; }    

    [Required(ErrorMessage = "Password é obrigatório")]
    public string? Password { get; set; }
}
