using System.Collections.Generic;
using System.Text;

namespace RefactoringKata
{
	public class OrdersWriter
	{
		private readonly Orders _orders;

		public OrdersWriter(Orders orders)
		{
			_orders = orders;
		}

		public string GetContents()
		{
			var sb = new StringBuilder("{\"orders\": [");

			SetOrderRelatedDatas(sb);

			if (_orders.GetOrdersCount() > 0)
			{
				sb.Remove(sb.Length - 2, 2);
			}

			return sb.Append("]}").ToString();
		}

		private void SetOrderRelatedDatas(StringBuilder sb)
		{
			for (var i = 0; i < _orders.GetOrdersCount(); i++)
			{
				var order = _orders.GetOrder(i);
				sb.Append("{");
				sb.Append("\"id\": ");
				sb.Append(order.GetOrderId());
				sb.Append(", ");
				sb.Append("\"products\": [");

				SetProductRelatedDatas(order, sb);

				if (order.GetProductsCount() > 0)
				{
					sb.Remove(sb.Length - 2, 2);
				}

				sb.Append("]");
				sb.Append("}, ");
			}
		}

		private static void SetProductRelatedDatas(Order order, StringBuilder sb)
		{
			for (var j = 0; j < order.GetProductsCount(); j++)
			{
				var product = order.GetProduct(j);
				TextExchangeHelper.SetProduct(product);

				var stringOrder = new string[] { "{", "\"code\": \"", product.Code, "\", ", "\"color\": \"", TextExchangeHelper.GetColorName(), "\", " };
				AppendStringWithArray(sb, stringOrder);

				if (product.Size != Product.SIZE_NOT_APPLICABLE)
				{
					sb.Append("\"size\": \"");
					sb.Append(TextExchangeHelper.GetSizeName());
					sb.Append("\", ");
				}

				sb.Append("\"price\": ");
				sb.Append(product.Price);
				sb.Append(", ");
				sb.Append("\"currency\": \"");
				sb.Append(product.Currency);
				sb.Append("\"}, ");
			}
		}

		private static void AppendStringWithArray(StringBuilder sb, IEnumerable<string> stringOrder)
		{
			foreach (var dataFormatString in stringOrder)
			{
				sb.Append(dataFormatString);
			}
		}
	}
}