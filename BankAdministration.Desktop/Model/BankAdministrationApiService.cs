using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BankAdministration.Persistence.DTOS;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;

namespace BankAdministration.Desktop.Model
{
    public class BankAdministrationApiService
    {
        private readonly HttpClient client_;

        public BankAdministrationApiService(string baseAddress)
        {
            client_ = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = name,
                Password = password
            };

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/Login/Login", user);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await client_.PostAsync("api/Login/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        
        public async Task<IEnumerable<UserDto>> LoadUsersAsync()
        {
            
            HttpResponseMessage response = await client_.GetAsync("api/Users/GetUsersForAdmin");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<UserDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<BankAccountDto>> LoadBankAccountsAsync(string userName)
        {
            HttpResponseMessage response = await client_.GetAsync(
                        QueryHelpers.AddQueryString("api/BankAccounts/GetBankAccountsByUserName", "userName", userName));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<BankAccountDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<TransactionDto>> LoadTransactionsAsync(string bankAccountNumber)
        {
            HttpResponseMessage response = await client_.GetAsync(
                QueryHelpers.AddQueryString("api/Transactions", "bankAccountNumber", bankAccountNumber));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<TransactionDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async void SetBankAccountLock(bool locking, string bankAccountNumber)
        {
            var locdDto = new LockDto
            {
                Number = bankAccountNumber,
                IsLocked = locking
            };

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/BankAccounts/SetLock", locdDto);

           if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async void SetDeposit(string bankAccountNumber, Int64 amount)
        {
            var dto = new DepositDto
            {
                DepositAmount = amount,
                Number = bankAccountNumber
            };

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/BankAccounts/SetDeposit", dto);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async void SetWithdrawn(string bankAccountNumber, Int64 amount)
        {
            var dto = new WithdrawnDto
            {
                WithdrawnAmount = amount,
                Number = bankAccountNumber
            };

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/BankAccounts/SetWithdrawn", dto);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async void SetTransfer(string bankAccountNumber, string destBankAccountNumber, string destUserName, Int64 amount)
        {
            var dto = new TransferDto
            {
                TransferAmount = amount,
                SourceNumber = bankAccountNumber,
                DestNumber = destBankAccountNumber,
                DestUserName = destUserName
            };

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/BankAccounts/SetTransfer", dto);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}
