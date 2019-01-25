using System.Collections.Generic;
using System.Linq;

namespace TheSongList.Models
{
    public class PageInfo
    {
        const int FIRST_PAGE = 1;

        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; } = 50;
        public int CurrentPage { get; set; }

        public int MaxPage => (TotalResults + ResultsPerPage - 1) / ResultsPerPage;
        public IEnumerable<int> Links => Enumerable.Range(FIRST_PAGE, MaxPage).Where(ShouldDisplay);
        public int FirstResult => ResultsPerPage * (CurrentPage - 1) + 1;
        public int LastResult => FirstResult + ResultsPerPage;


        private bool ShouldDisplay(int pageNumber) =>
            pageNumber == FIRST_PAGE || pageNumber == MaxPage || (pageNumber >= CurrentPage - 3 && pageNumber <= CurrentPage + 3);
    }
}