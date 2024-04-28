using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Mnimum Password Length is 5")]
		[DataType(DataType.Password)] //34an yzhar k astreks
		public string NewPassword { get; set; }



		[Required(ErrorMessage = "Password is Required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password not matched Password")]
		[DataType(DataType.Password)] //34an yzhar k astreks
		public string ConfirmPassword { get; set; }

        //public string token { get; set; }

        //public string Email { get; set; }
    }
}
