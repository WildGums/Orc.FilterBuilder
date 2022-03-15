namespace Orc.FilterBuilder.Tests
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel.IoC;
    using NUnit.Framework;
    using Orc.Automation;
    using ViewModels;
    using Views;
    using Condition = FilterBuilder.Condition;
    using PropertyMetadata = FilterBuilder.PropertyMetadata;

    [TestFixture]
    public class EditFilterViewFacts : StyledControlTestFacts<EditFilterView>
    {
        [Target]
        public Automation.EditFilterView Target { get; set; }

        [Test]
        public void Correctly()
        {
            var target = Target;
            var model = target.Current;

            var filterScheme = new FilterScheme(typeof(EditFilterItemTestClass));

            var conditionGroup = new ConditionGroup();
            var propertyExpression = new PropertyExpression
            {
                Property = new PropertyMetadata(typeof(EditFilterItemTestClass), typeof(EditFilterItemTestClass).GetProperty(nameof(EditFilterItemTestClass.Field1))),
                DataTypeExpression = new IntegerExpression
                {
                    Value = 1,
                    ValueControlType = ValueControlType.Integer,
                    SelectedCondition = Condition.EqualTo
                }
            };

            conditionGroup.Items.Add(propertyExpression);
            filterScheme.Root.Items.Add(propertyExpression);

            target.Execute<InitializeEditFilterViewMethodRun>(filterScheme);
        }
    }

    public class EditFilterItemTestClass
    {
        public int Field1 { get; set; }
        public string Field2 { get; set; }
        public double Field3 { get; set; }
    }

    public class InitializeEditFilterViewMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.NotSetValue;

            var filterScheme = method.Parameters[0].ExtractValue() as FilterScheme;

            if (owner is not EditFilterView editFilterView)
            {
                return false;
            }
            
            var filterSchemeEditInfo = new FilterSchemeEditInfo(filterScheme, 
                new List<EditFilterItemTestClass>
                {
                    new() 
                    {
                        Field1 = 1,
                        Field2 = "Record 1",
                        Field3 = 1.0
                    },

                    new()
                    {
                        Field1 = 2,
                        Field2 = "Record 2",
                        Field3 = 2.0d
                    },  
                    
                    new()
                    {
                        Field1 = 3,
                        Field2 = "Record 3",
                        Field3 = 3.0d
                    },
                }, 
                true, true);

            var typeFactory = this.GetTypeFactory();

            var editFilterViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<EditFilterViewModel>(filterSchemeEditInfo);
            editFilterView.DataContext = editFilterViewModel;

            result = AutomationValue.FromValue(true);

            return true;
        }
    }
}
