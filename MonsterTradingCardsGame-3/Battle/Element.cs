using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame_3.Battle
{
    internal class Element
    {
        public Element() { }

        private string _elementType;
        private string _inferiorelement;

        public string ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        public string InferiorElementType
        {
            get { return _inferiorelement; }
            set { _inferiorelement = value; }
        }
    }
}
