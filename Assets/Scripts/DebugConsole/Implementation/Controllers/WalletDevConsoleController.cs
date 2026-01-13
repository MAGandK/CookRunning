using System;
using Constants;
using Services.Price;
using Services.Storage;
using SRDebugger;

namespace DebugConsole.Controllers
{
    public class WalletDevConsoleController : IDevConsoleController
    {
        private const string CategoryName = "Currency";
        public int CroupPriority => 0;
        private readonly WalletStorageData _walletStorageData;
        private int _currencyTypeIndex;
        private readonly CurrencyType[] _currencyTypes;

        public WalletDevConsoleController(IStorageService storageService)
        {
            _walletStorageData = storageService.GetData<WalletStorageData>(StorageDataNames.WALLET_STORAGE_DATA_KEY);
            _currencyTypes = (CurrencyType[])Enum.GetValues(typeof(CurrencyType));
        }
        

        public void Init()
        {
            var service = SRDebug.Instance;

            var dynamicOptionContainer = new DynamicOptionContainer();

            dynamicOptionContainer.AddOption(AddCurrency(CurrencyType.coin, 100));
            dynamicOptionContainer.AddOption(AddCurrency(CurrencyType.rybi, 100));
            dynamicOptionContainer.AddOption(AddCurrency(CurrencyType.coin, -100));
            dynamicOptionContainer.AddOption(AddCurrency(CurrencyType.rybi, -100));
            dynamicOptionContainer.AddOption(OptionDefinition.FromMethod("Add a lot of", AddAllCurrency,  category: CategoryName)); 
            dynamicOptionContainer.AddOption(OptionDefinition.FromMethod("Reset all", ResetAll,  category: CategoryName));

            service.AddOptionContainer(dynamicOptionContainer);
        }

        private void ResetAll()
        {
            foreach (var currencyType in _currencyTypes)
            {
                _walletStorageData.AddCurrency(currencyType, -_walletStorageData.GetBalance(currencyType));
            }
        }

        private void AddAllCurrency()
        {
            foreach (var currencyType in _currencyTypes)
            {
                _walletStorageData.AddCurrency(currencyType, 10000);
            }
        }

        private OptionDefinition AddCurrency(CurrencyType currencyType, int value)
        {
            var optionDefinition = OptionDefinition.FromMethod($"Add {value} {currencyType}",
                () => _walletStorageData.AddCurrency(currencyType, value), category: CategoryName);
            return optionDefinition;
        }
    }
}