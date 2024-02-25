using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// the unique identifier for the prize
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the place number, such as first as 1, second as 2, etc.
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Represents the special name of each place, such as champion for the first place.
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Represents the fixed prize amount, such as $100 for the first place, 
        /// $50 for the second place
        /// </summary>
        public decimal PrizeAmount { get; set; }
        /// <summary>
        /// Represents the percentage of the prize, such as 50% of the whole income, if the whole
        /// tournament income is $200, the prize will be 50% * $200 = $100
        /// </summary>
        public double PrizePercentage { get; set; }
        public PrizeModel()
        {
                
        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            PrizePercentage = prizePercentageValue;


        }
    }
}
