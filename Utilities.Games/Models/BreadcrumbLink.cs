using System.Text.RegularExpressions;

namespace Utilities.Games.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://stackoverflow.com/a/64721882/4585104"/>
    public class BreadcrumbLink
    {
        public int OrderIndex { get; set; }

        public string Address { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }
    }
    public class BreadcrumbConfig
    {
        public string Match { get; set; }
        public string Title { get; set; }
        private Regex _reg;
        public Regex MatchExpression
        {
            get
            {
                if (_reg == null) {
                    _reg = new Regex($"^{Match}$");
                }
                return _reg;
            }
        }

        public bool IsMatch(string address) => MatchExpression.IsMatch(address);

        public string FormatTitle(string address)
        {
            return Regex.Replace(address, MatchExpression.ToString(), Title);
        }
    }
}
