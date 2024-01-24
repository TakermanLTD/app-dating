using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Data
{
    public enum DateType
    {
        [Display(Name = "Онлайн")]
        Online = 1,

        [Display(Name = "На място")]
        OnPremise = 2
    }
}