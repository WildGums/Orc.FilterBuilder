using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Orc.FilterBuilder
{
	public class FilterSchemeManager
	{
		private InstanceProperties _instanceProperties;

		private const string FileName = "filterSchemes.xml";

		private const string FilterElement = "Filter";
		private const string ConditionGroupElement = "ConditionGroup";
		private const string PropertyExpressionElement = "PropertyExpression";
		private const string SchemesElement = "Schemes";

		private const string TitleAttribute = "Title";
		private const string TypeAttribute = "Type";
		private const string NameAttribute = "Name";
		private const string ConditionAttribute = "Condition";
		private const string SelectedSpanTypeAttribute = "SelectedSpanType";
		private const string AmountAttribute = "Amount";
		private const string ValueAttribute = "Value";
		private const string IsNullableAttribute = "IsNullable";

		#region Save

		public void Save(IEnumerable<FilterScheme> filterSchemes)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			XmlWriter xmlWriter = XmlWriter.Create(FileName, settings);
			xmlWriter.WriteStartElement(SchemesElement);
			foreach (FilterScheme filterScheme in filterSchemes)
				SaveFilterScheme(xmlWriter, filterScheme);
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			xmlWriter.Close();
		}

		private void SaveFilterScheme(XmlWriter xmlWriter, FilterScheme filterScheme)
		{
			xmlWriter.WriteStartElement(FilterElement);
			xmlWriter.WriteAttributeString(TitleAttribute, filterScheme.Title);
			SaveConditionTreeItem(xmlWriter, filterScheme.Root);
			xmlWriter.WriteEndElement();
		}

		private void SaveConditionTreeItem(XmlWriter xmlWriter, ConditionTreeItem item)
		{
			if (item is ConditionGroup)
				SaveConditionGroup(xmlWriter, item as ConditionGroup);
			else
				SavePropertyExpression(xmlWriter, item as PropertyExpression);

			foreach (ConditionTreeItem children in item.Items)
				SaveConditionTreeItem(xmlWriter, children);

			xmlWriter.WriteEndElement();
		}

		private void SaveConditionGroup(XmlWriter xmlWriter, ConditionGroup conditionGroup)
		{
			xmlWriter.WriteStartElement(ConditionGroupElement);
			xmlWriter.WriteAttributeString(TypeAttribute, conditionGroup.Type.ToString());
		}

		private void SavePropertyExpression(XmlWriter xmlWriter, PropertyExpression propertyExpression)
		{
			xmlWriter.WriteStartElement(PropertyExpressionElement);
			xmlWriter.WriteAttributeString(NameAttribute, propertyExpression.Property.Name);
			xmlWriter.WriteAttributeString(ConditionAttribute, propertyExpression.DataTypeExpression.SelectedCondition.ToString());

			if (propertyExpression.DataTypeExpression is TimeSpanExpression)
			{
				TimeSpanExpression expr = propertyExpression.DataTypeExpression as TimeSpanExpression;
				xmlWriter.WriteAttributeString(SelectedSpanTypeAttribute, expr.SelectedSpanType.ToString());
				xmlWriter.WriteAttributeString(AmountAttribute, expr.Amount.ToString());
			}
			else if (propertyExpression.DataTypeExpression is DateTimeExpression)
			{
				DateTimeExpression expr = propertyExpression.DataTypeExpression as DateTimeExpression;
				xmlWriter.WriteAttributeString(IsNullableAttribute, expr.IsNullable.ToString());
				xmlWriter.WriteAttributeString(ValueAttribute, expr.Value.ToString());
			}
			else if (propertyExpression.DataTypeExpression is DecimalExpression)
			{
				DecimalExpression expr = propertyExpression.DataTypeExpression as DecimalExpression;
				xmlWriter.WriteAttributeString(IsNullableAttribute, expr.IsNullable.ToString());
				xmlWriter.WriteAttributeString(ValueAttribute, expr.Value.ToString());
			}
			else if (propertyExpression.DataTypeExpression is StringExpression)
			{
				StringExpression expr = propertyExpression.DataTypeExpression as StringExpression;
				xmlWriter.WriteAttributeString(ValueAttribute, expr.Value);
			}
			else if (propertyExpression.DataTypeExpression is BooleanExpression)
			{
				BooleanExpression expr = propertyExpression.DataTypeExpression as BooleanExpression;
				xmlWriter.WriteAttributeString(ValueAttribute, expr.Value.ToString());
			}
			else if (propertyExpression.DataTypeExpression is IntegerExpression)
			{
				IntegerExpression expr = propertyExpression.DataTypeExpression as IntegerExpression;
				xmlWriter.WriteAttributeString(IsNullableAttribute, expr.IsNullable.ToString());
				xmlWriter.WriteAttributeString(ValueAttribute, expr.Value.ToString());
			}
			else
				throw new NotSupportedException(propertyExpression.DataTypeExpression.GetType().ToString());
		}

		#endregion

		#region Load

		public IEnumerable<FilterScheme> Load(InstanceProperties instanceProperties)
		{
			_instanceProperties = instanceProperties;

			List<FilterScheme> filterSchemes = new List<FilterScheme>();
			if (File.Exists(FileName))
			{
				XmlReader xmlReader = XmlReader.Create(new StreamReader(FileName));
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == FilterElement)
					{
						filterSchemes.Add(ReadFilterScheme(xmlReader));
					}
				}
				xmlReader.Close();
			}
			return filterSchemes;
		}

		private FilterScheme ReadFilterScheme(XmlReader xmlReader)
		{
			string title = xmlReader.GetAttribute(TitleAttribute);
			FilterScheme filterScheme = null;
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == ConditionGroupElement)
				{
					ConditionGroup conditionGroup = ReadConditionGroup(xmlReader);
					filterScheme = new FilterScheme(title, conditionGroup);
				}

				if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == FilterElement)
					break;
			}
			return filterScheme;
		}

		private ConditionGroup ReadConditionGroup(XmlReader xmlReader)
		{
			ConditionGroup conditionGroup = new ConditionGroup();
			conditionGroup.Type = ParseEnum<ConditionGroupType>(xmlReader.GetAttribute(TypeAttribute));

			if (!xmlReader.IsEmptyElement)
			{
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						ConditionTreeItem item;
						if (xmlReader.Name == ConditionGroupElement)
							item = ReadConditionGroup(xmlReader);
						else
							item = ReadPropertyExpression(xmlReader);
						item.Parent = conditionGroup;
						conditionGroup.Items.Add(item);
					}
					else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == ConditionGroupElement)
					{
						break;
					}
				}
			}
			return conditionGroup;
		}

		private T ParseEnum<T>(string value)
		{
			return (T) Enum.Parse(typeof (T), value);
		}

		private PropertyExpression ReadPropertyExpression(XmlReader xmlReader)
		{
			PropertyExpression propertyExpression = new PropertyExpression();
			propertyExpression.Property = _instanceProperties.GetProperty(xmlReader.GetAttribute(NameAttribute));
			propertyExpression.DataTypeExpression.SelectedCondition = ParseEnum<Condition>(xmlReader.GetAttribute(ConditionAttribute));

			if (propertyExpression.DataTypeExpression is TimeSpanExpression)
			{
				TimeSpanExpression expr = propertyExpression.DataTypeExpression as TimeSpanExpression;
				expr.SelectedSpanType = ParseEnum<TimeSpanType>(xmlReader.GetAttribute(SelectedSpanTypeAttribute));
				expr.Amount = float.Parse(xmlReader.GetAttribute(AmountAttribute));
			}
			else if (propertyExpression.DataTypeExpression is DateTimeExpression)
			{
				DateTimeExpression expr = propertyExpression.DataTypeExpression as DateTimeExpression;
				expr.IsNullable = bool.Parse(xmlReader.GetAttribute(IsNullableAttribute));
				expr.Value = DateTime.Parse(xmlReader.GetAttribute(ValueAttribute));
			}
			else if (propertyExpression.DataTypeExpression is DecimalExpression)
			{
				DecimalExpression expr = propertyExpression.DataTypeExpression as DecimalExpression;
				expr.IsNullable = bool.Parse(xmlReader.GetAttribute(IsNullableAttribute));
				expr.Value = decimal.Parse(xmlReader.GetAttribute(ValueAttribute));
			}
			else if (propertyExpression.DataTypeExpression is StringExpression)
			{
				StringExpression expr = propertyExpression.DataTypeExpression as StringExpression;
				expr.Value = xmlReader.GetAttribute(ValueAttribute);
			}
			else if (propertyExpression.DataTypeExpression is BooleanExpression)
			{
				BooleanExpression expr = propertyExpression.DataTypeExpression as BooleanExpression;
				expr.Value = bool.Parse(xmlReader.GetAttribute(ValueAttribute));
			}
			else if (propertyExpression.DataTypeExpression is IntegerExpression)
			{
				IntegerExpression expr = propertyExpression.DataTypeExpression as IntegerExpression;
				expr.IsNullable = bool.Parse(xmlReader.GetAttribute(IsNullableAttribute));
				expr.Value = int.Parse(xmlReader.GetAttribute(ValueAttribute));
			}
			else
				throw new NotSupportedException(propertyExpression.DataTypeExpression.GetType().ToString());

			return propertyExpression;
		}

		#endregion

	}
}
