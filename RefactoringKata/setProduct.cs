using System;
using RefactoringKata.ProductTypeEnum;

namespace RefactoringKata
{
	public static class TextExchangeHelper
	{
		private static Product _product;

		public static void SetProduct(Product product)
		{
			_product = product;
		}

		public static string GetSizeFor()
		{
			return GetTypeName(typeof(ProductSize), _product.Size, "Invalid Size");
		}

		public static string GetColorFor()
		{
			return GetTypeName(typeof(ProductColor), _product.Color, "no color").ToLower();
		}

		private static string GetTypeName(Type targetEnumType, int typeNum, string defaultName)
		{
			return Enum.IsDefined(targetEnumType, typeNum) ? Enum.GetName(targetEnumType, typeNum) : defaultName;
		}
	}
}