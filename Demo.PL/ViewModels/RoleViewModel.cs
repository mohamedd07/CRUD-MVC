using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RoleViewModeL
    {
        public string Id { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public RoleViewModeL()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

}
