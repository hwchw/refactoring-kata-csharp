using System.Collections.Generic;
using System.Text;

namespace RefactoringKata
{
	public class OrdersWriter
	{
		private readonly Orders _orders;
		private static StringBuilder _sb;

		public OrdersWriter(Orders orders)
		{
			_orders = orders;
			_sb = new StringBuilder();
		}

		public string GetContents()
		{
			_sb.Append("{\"orders\": [");
			if (_orders.GetOrdersCount() > 0)
			{
				AppendOrdersToStringBuilder();
			}
			_sb.Append("]}");
			return _sb.ToString();
		}

		private void AppendOrdersToStringBuilder()
		{
			for (var i = 0; i < _orders.GetOrdersCount(); i++)
			{
				var orderDataDict = GetOrderAsDictionary(_orders.GetOrder(i));
				_sb.Append("{");
				AppendStringDictionaryToStringBuilder(orderDataDict);
				AppendProductToStringBuilder(_orders.GetOrder(i));
				_sb.Append("}, ");
			}
			RemoveLastPeriod();
		}

		private static void AppendProductToStringBuilder(Order order)
		{
			_sb.Append(", \"products\": [");
			for (var j = 0; j < order.GetProductsCount(); j++)
			{
				var product = order.GetProduct(j);
				TextExchangeHelper.SetProduct(product);
				var productDataDict = GetSingleProductAsDictionary(product);
				_sb.Append("{");
				AppendStringDictionaryToStringBuilder(productDataDict);
				_sb.Append("}");
			}
			_sb.Append("]");
		}

		private static Dictionary<string, string> GetOrderAsDictionary(Order order)
		{
			var productSymbolDict = new Dictionary<string, string>
			{
				{"id", order.GetOrderId().ToString()},
			};
			return productSymbolDict;
		}

		private static Dictionary<string, string> GetSingleProductAsDictionary(Product product)
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

		private static void AppendStringDictionaryToStringBuilder(Dictionary<string, string> targetDictionary)
		{
			foreach (var stringPair in targetDictionary)
			{
				_sb.AppendFormat(IsFloatOrInt(stringPair.Value) ? "\"{0}\": {1}, " : "\"{0}\": \"{1}\", ", stringPair.Key, stringPair.Value);
			}
			RemoveLastPeriod();
		}

		private static void RemoveLastPeriod()
		{
			_sb.Remove(_sb.Length - 2, 2);
		}

		private static bool IsFloatOrInt(string targetString)
		{
			float f;
			int i;
			return float.TryParse(targetString, out f) || int.TryParse(targetString, out i);
		}
	}
}