using it.unical.mat.embasp.languages.asp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using UnityEngine;

namespace ThinkEngine.Planning
{
    class ASPPlannerBrain : IActualPlannerBrain
    {
        Plan plan;
        private bool answerSetAvailable;
        private AnswerSet currentAnswerSet;
        private SortedDictionary<int, Action> _actions;
        private Dictionary<int, List<KeyValuePair<string, string>>> _actionParameters;
        private Dictionary<int, List<KeyValuePair<string, string>>> ActionParameters
        {
            get
            {
                if (_actionParameters == null)
                {
                    _actionParameters = new Dictionary<int, List<KeyValuePair<string, string>>>();
                }
                return _actionParameters;
            }
        }

        private SortedDictionary<int, Action> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = new SortedDictionary<int, Action>();
                }
                return _actions;
            }
        }
        internal void AnswerSetAvailable(AnswerSet answerSet)
        {
            answerSetAvailable = true;
            currentAnswerSet = answerSet;
        }


        private List<Action> Parse(AnswerSet answerSet)
        {
            _actionParameters = new Dictionary<int, List<KeyValuePair<string, string>>>();
            _actions = new SortedDictionary<int, Action>();
            foreach (string literal in answerSet.GetAnswerSet())
            {
                if (literal.StartsWith("applyAction("))
                {
                    ParseApplyActionLiteral(literal);
                }
                else if (literal.StartsWith("actionArgument("))
                {
                    ParseActionArgumentLiteral(literal);
                }
            }
            AssociateActionsToParameters();
            List<Action> toReturn = Actions.Values.ToList();
            toReturn.Sort();
            return toReturn;
        }

        private void AssociateActionsToParameters()
        {
            foreach (int order in ActionParameters.Keys)
            {
                if (Actions.ContainsKey(order))
                {
                    foreach (KeyValuePair<string, string> parameter in ActionParameters[order])
                    {
                        PropertyInfo property = Actions[order].GetType().GetProperty(parameter.Key, Utility.BindingAttr);
                        if (property != null)
                        {
                            Type propertyType = property.PropertyType;
                            property.SetValue(Actions[order], Convert.ChangeType(parameter.Value, propertyType));
                        }
                        else
                        {
                            Debug.Log("Property " + parameter.Key + " not found in class " + Actions[order].GetType().Name);
                        }
                    }
                }
                else
                {
                    Debug.Log("Action with order of execution " + order + " not set.");
                }
            }
        }

        private void ParseActionArgumentLiteral(string literal)
        {
            string partial = literal.Remove(0, "actionArgument(".Count());
            string toParse = partial.Substring(0, partial.IndexOf(","));
            bool converted = int.TryParse(toParse, out int order);
            if (converted)
            {
                partial = partial.Remove(0, partial.IndexOf(",") + 1);
                string parameterName = partial.Substring(0, partial.IndexOf(",")).Trim('\"');
                partial = partial.Remove(0, partial.IndexOf(",") + 1);
                string parameterValue = partial.Substring(0, partial.IndexOf(")")).Trim('\"');
                if (!ActionParameters.ContainsKey(order))
                {
                    ActionParameters[order] = new List<KeyValuePair<string, string>>();
                }
                ActionParameters[order].Add(new KeyValuePair<string, string>(parameterName, parameterValue));
            }
        }

        private void ParseApplyActionLiteral(string literal)
        {
            string partial = literal.Remove(0, "applyAction(".Count());
            string toParse = partial.Substring(0, partial.IndexOf(","));
            bool converted = int.TryParse(toParse, out int order);
            if (converted)
            {
                partial = partial.Remove(0, partial.IndexOf(",") + 1);
                string actionClass = partial.Substring(0, partial.IndexOf(")")).Trim('\"');
                Actions[order] = ScriptableObject.CreateInstance(actionClass) as Action;
                if (!ActionParameters.ContainsKey(order))
                {
                    ActionParameters[order] = new List<KeyValuePair<string, string>>();
                }
            }
        }
        internal Plan GetPlan()
        {
            return plan;
        }

        public bool IsNewPlanAvailable()
        {
            return answerSetAvailable;
        }

        public Plan GetNewPlan()
        {
            if (answerSetAvailable)
            {
                answerSetAvailable = false;
                plan = new Plan(Parse(currentAnswerSet));
                return GetPlan();
            }
            return new Plan();
        }

        public ASPExecutor GetPlannerExecutor(PlannerBrain plannerBrain)
        {
            return new ASPPlannerExecutor(plannerBrain);
        }

        public void NewPlanAvailable(object plan)
        {
            if (!(plan is AnswerSet))
            {
                return;
            }
            AnswerSetAvailable((AnswerSet)plan);
        }

        public string ActualSensorEncoding(string sensorsAsASP)
        {
            return sensorsAsASP;
        }

        public string SpecificFileParts()
        {
            string toReturn = "";
            toReturn += "% Predicates for Action invokation.\n";
            toReturn += "% applyAction(OrderOfExecution,ActionClassName).\n";
            toReturn += "% actionArgument(ActionOrder,ArgumentName, ArgumentValue).\n";
            return toReturn;
        }
    }
}
