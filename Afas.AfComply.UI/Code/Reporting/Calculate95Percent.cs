using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Code.Reporting
{
    public class Calculate95Percent
    {
        private List<Percent95Data> allMonths = new List<Percent95Data>();

        public Calculate95Percent(int employerId)
        {

        }

        public List<GraphView> Pivot()
        {
            List<GraphView> pivot = new List<GraphView>();
            GraphView TotalFTE = new GraphView();
            TotalFTE.Title = "FTE Total";
            pivot.Add(TotalFTE);
            GraphView InsureFTE = new GraphView();
            InsureFTE.Title = "FTE Insure";
            pivot.Add(InsureFTE);
            GraphView Percent = new GraphView();
            Percent.Title = "Percent";
            pivot.Add(Percent);

            foreach (Percent95Data month in allMonths)
            {
                TotalFTE.SetMonth(month.Month, month.TotalFTEs.ToString());
                InsureFTE.SetMonth(month.Month, month.FTEsOfferedIns.ToString());
                Percent.SetMonth(month.Month, month.Percent.ToString());
            }

            return pivot;
        }
    }

    /// <summary>
    /// This class exists to show data in the graph even though the data doesn't fit this format
    /// </summary>
    public class GraphView 
    {
        public string Title { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public void SetMonth(int monthId, string data) 
        {
            if (monthId == 1)      { Jan = data; }
            else if (monthId == 2) { Feb = data; }
            else if (monthId == 3) { Mar = data; }
            else if (monthId == 4) { Apr = data; }
            else if (monthId == 5) { May = data; }
            else if (monthId == 6) { Jun = data; }
            else if (monthId == 7) { Jul = data; }
            else if (monthId == 8) { Aug = data; }
            else if (monthId == 9) { Sep = data; }
            else if (monthId == 10) { Oct = data; }
            else if (monthId == 11) { Nov = data; }
            else if (monthId == 12) { Dec = data; }
        }
    }
}