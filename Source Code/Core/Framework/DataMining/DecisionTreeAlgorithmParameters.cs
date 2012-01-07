using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobZoom.Core.Framework.DataMining
{
    #region Algorithm Parameters

    public class DecisionTreeAlgorithmParameters
    {
        private int _HoldoutMaxPercent = 10; //Default .Net 30%
        private int _SCORE_METHOD = 4; //Entropy (1), Bayesian with K2 Prior (2), or Bayesian Dirichlet Equivalent (BDE) Prior (4 - .Net Default)
        private float _COMPLEXITY_PENALTY = 0.1f; //Default 0.5
        private int _SPLIT_METHOD = 3; //.Net Default 3
        private int _MAXIMUM_INPUT_ATTRIBUTES = 255; //.Net Default 255
        private int _MAXIMUM_OUTPUT_ATTRIBUTES = 255; //.NetDefault 255
        private float _MINIMUM_SUPPORT = 0.05f; //.NetDefault 10

        /// <summary>
        /// Holdout Max Percent - Default is 10 (10%)
        /// </summary>
        public int HoldoutMaxPercent { get { return _HoldoutMaxPercent; } }

        /// <summary>
        /// SCORE_METHOD value must be 1 / 2 / 4 (default)
        /// </summary>
        public int SCORE_METHOD { get { return _SCORE_METHOD; } }

        /// <summary>
        /// COMPLEXITY_PENALTY - default: 0.1
        /// </summary>
        public float COMPLEXITY_PENALTY { get { return _COMPLEXITY_PENALTY; } }

        /// <summary>
        /// SPLIT_METHOD - default 3
        /// </summary>
        public int SPLIT_METHOD { get { return _SPLIT_METHOD; } }

        /// <summary>
        /// MAXIMUM_INPUT_ATTRIBUTES - default 255
        /// </summary>
        public int MAXIMUM_INPUT_ATTRIBUTES { get { return _MAXIMUM_INPUT_ATTRIBUTES; } }

        /// <summary>
        /// MAXIMUM_OUTPUT_ATTRIBUTES - default 255
        /// </summary>
        public int MAXIMUM_OUTPUT_ATTRIBUTES { get { return _MAXIMUM_OUTPUT_ATTRIBUTES; } }

        /// <summary>
        /// MINIMUM_SUPPORT - default 0.05
        /// </summary>
        public float MINIMUM_SUPPORT { get { return _MINIMUM_SUPPORT; } }

        public DecisionTreeAlgorithmParameters(int HoldoutMaxPercent, int SCORE_METHOD, float COMPLEXITY_PENALTY, int SPLIT_METHOD, int MAXIMUM_INPUT_ATTRIBUTES, int MAXIMUM_OUTPUT_ATTRIBUTES, float MINIMUM_SUPPORT)
        {
            if (SCORE_METHOD != 1 || SCORE_METHOD != 2 || SCORE_METHOD != 4)
            {
                SCORE_METHOD = 4;
            }
            _HoldoutMaxPercent = HoldoutMaxPercent;
            _SCORE_METHOD = SCORE_METHOD;
            _COMPLEXITY_PENALTY = COMPLEXITY_PENALTY;
            _SPLIT_METHOD = SPLIT_METHOD;
            _MAXIMUM_INPUT_ATTRIBUTES = MAXIMUM_INPUT_ATTRIBUTES;
            _MAXIMUM_OUTPUT_ATTRIBUTES = MAXIMUM_OUTPUT_ATTRIBUTES;
            _MINIMUM_SUPPORT = MINIMUM_SUPPORT;
        }

        /// <summary>
        /// DecisionTree Mining with default AlgorithmParameters
        /// </summary>
        public DecisionTreeAlgorithmParameters()
        {
            _HoldoutMaxPercent = 10;
            _SCORE_METHOD = 4;
            _COMPLEXITY_PENALTY = 0.1f;
            _SPLIT_METHOD = 3;
            _MAXIMUM_INPUT_ATTRIBUTES = 255;
            _MAXIMUM_OUTPUT_ATTRIBUTES = 255;
            _MINIMUM_SUPPORT = 0.05f;
        }
    }

    #endregion
}
