using Microsoft.EntityFrameworkCore;

namespace FATWA_DOMAIN.Models.ViewModel
{
    [Keyless]
    public class LegalLibraryVM
    {
        public List<LmsLiterature>? Books { get; set; }
    }
}
