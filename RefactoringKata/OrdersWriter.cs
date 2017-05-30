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

			return sb.Append("]}").ToString();
		}

		private void SetOrderRelatedDatas(StringBuilder sb)
		{
			for (var i = 0; i < _orders.GetOrdersCount(); i++)
			{
				var order = _orders.GetOrder(i);
				var tempStringOrder = new[] { "{", "\"id\": ", order.GetOrderId().ToString(), ", ", "\"products\": [" };
				AppendStringWithArray(sb, tempStringOrder);

				SetProductRelatedDatas(order, sb);

				if (order.GetProductsCount() > 0)
				{
					sb.Remove(sb.Length - 2, 2);
				}

				sb.Append("]}, ");
			}
			if (_orders.GetOrdersCount() > 0)
			{
				sb.Remove(sb.Length - 2, 2);
			}
		}

		private static void SetProductRelatedDatas(Order order, StringBuilder sb)
		{
			for (var j = 0; j < order.GetProductsCount(); j++)
			{
				var product = order.GetProduct(j);
				TextExchangeHelper.SetProduct(product);

				var productRelativeStringOrder = new[] { "{", "\"code\": \"", product.Code, "\", ", "\"color\": \"", TextExchangeHelper.GetColorName(), "\", " };
				AppendStringWithArray(sb, productRelativeStringOrder);

				if (product.Size != Product.SIZE_NOT_APPLICABLE)
				{
					var sizeRelativeStringOrder = new[] { "\"size\": \"", TextExchangeHelper.GetSizeName(), "\", " };
					AppendStringWithArray(sb, sizeRelativeStringOrder);
				}

				var productRelativeEndingStringOrder = new[] { "\"price\": ", product.Price + "", ", ", "\"currency\": \"", product.Currency, "\"}, " };
				AppendStringWithArray(sb, productRelativeEndingStringOrder);
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