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

            HttpResponseMessage response = await client_.PostAsJsonAsync("api/User/Login", user);

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
            HttpResponseMessage response = await client_.PostAsync("api/User/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<BankAccountDto>> LoadBankAccountsAsync(string userName)
        {
            HttpResponseMessage response = await client_.GetAsync(
                        QueryHelpers.AddQueryString("api/BankAccounts", "userName", userName));

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
    }
}
