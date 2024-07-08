using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Data
{
    public enum Ethnicity
    {
        [Display(Name = "Всякакви")]
        All = 1,

        [Display(Name = "Българин")]
        Bulgar = 2,

        [Display(Name = "Ром")]
        Rom = 3,

        [Display(Name = "Копанарин")]
        Kopanar = 4,

        [Display(Name = "Турчин")]
        Turk = 5,

        [Display(Name = "Помак")]
        Pomak = 6,

        [Display(Name = "Славянин")]
        Slav = 7
    }
}