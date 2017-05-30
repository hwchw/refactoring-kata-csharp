using System.Collections.Generic;
using System.Text;

namespace RefactoringKata
{
	public class OrdersWriter
	{
		private readonly Orders _orders;
		Dictionary<string, string> _info = new Dictionary<string, string>();
		public OrdersWriter(Orders orders)
		{
			_orders = orders;
		}

		public string GetContents()
		{
			var sb = new StringBuilder("{\"orders\": [");

			SetOrderRelatedDatas(sb);

			return sb.Append("]}").ToString();
		}


		private void SetOrderRelatedDatas(StringBuilder sb)
		{
			for (var i = 0; i < _orders.GetOrdersCount(); i++)
			{
				var order = _orders.GetOrder(i);
				var tempStringOrder = new[] { "{", "\"id\": ", order.GetOrderId().ToString(), ", " };
				AppendStringWithArray(sb, tempStringOrder);

				SetProductRelatedDatas(order, sb);

				sb.Append("]}, ");
			}
			if (_orders.GetOrdersCount() > 0)
			{
				sb.Remove(sb.Length - 2, 2);
			}
		}

		private static void SetProductRelatedDatas(Order order, StringBuilder sb)
		{
			sb.Append("\"products\": [");
			for (var j = 0; j < order.GetProductsCount(); j++)
			{
				var product = order.GetProduct(j);
				TextExchangeHelper.SetProduct(product);
				var productDataDict = GetOneProductAsJsonArray(product);
				AppendStringDictionaryToStringBuilder(sb, productDataDict);
			}
		}

		private static Dictionary<string, string> GetOneProductAsJsonArray(Product product)
		{
			var productSymbolDict = new Dictionary<string, string>
			{
				{"code", product.Code},
				{"color", TextExchangeHelper.GetColorName()},
				{"size", TextExchangeHelper.GetSizeName()},
				{"price", product.Price + ""},
				{"currency", product.Currency}
			};

			if (product.Size == Product.SIZE_NOT_APPLICABLE)
			{
				productSymbolDict.Remove("size");
			}

			return productSymbolDict;
		}

		private static void AppendStringDictionaryToStringBuilder(StringBuilder sb, Dictionary<string, string> targetDictionary)
		{
			sb.Append("{");
			foreach (var stringPair in targetDictionary)
			{
				sb.AppendFormat(IsFloat(stringPair.Value) ? "\"{0}\": {1}, " : "\"{0}\": \"{1}\", ", stringPair.Key, stringPair.Value);
			}
			sb.Remove(sb.Length-2,2);
			sb.Append("}");
		}

		private static void AppendStringWithArray(StringBuilder sb, IEnumerable<string> stringOrder)
		{
			foreach (var dataFormatString in stringOrder)
			{
				sb.Append(dataFormatString);
			}
		}

		private static bool IsFloat(string targetString)
		{
			float f;
			return float.TryParse(targetString, out f);
		}
	}
}