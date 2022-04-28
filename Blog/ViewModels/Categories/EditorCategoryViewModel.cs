using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Esse campo deve ter entre 3 e 40 caractéres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public string Slug { get; set; }
    }
}
