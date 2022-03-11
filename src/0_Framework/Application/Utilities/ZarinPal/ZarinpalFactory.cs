﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace _0_Framework.Application.ZarinPal;

public class ZarinPalFactory : IZarinPalFactory
{
    private readonly IConfiguration _configuration;

    private string Prefix { get; set; }
    //private string MerchantId { get; }

    public ZarinPalFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        Prefix = "sandbox"; //_configuration.GetSection("payment")["method"]
                            //MerchantId = _configuration.GetSection("payment")["merchant"];
    }

    public async Task<PaymentResponse> CreatePaymentRequest(string callBackUrl, string amount, string mobile, string email, long orderId)
    {
        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);

        var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentRequest.json");
        var request = new RestRequest(Method.POST);
        request.AddHeader("Content-Type", "application/json");
        var body = new PaymentRequest
        {
            Mobile = mobile,
            CallbackURL = $"{callBackUrl}/{orderId}",
            Description = $"خرید سفارش کد : {orderId}",
            Email = email,
            Amount = finalAmount
        };
        request.AddJsonBody(body);
        var response = await client.ExecuteAsync(request);

        return JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
    }

    public async Task<VerificationResponse> CreateVerificationRequest(string authority, string amount)
    {
        var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentVerification.json");
        var request = new RestRequest(Method.POST);
        request.AddHeader("Content-Type", "application/json");

        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);

        request.AddJsonBody(new VerificationRequest
        {
            Amount = finalAmount,
            Authority = authority
        });
        var response = await client.ExecuteAsync(request);

        return JsonConvert.DeserializeObject<VerificationResponse>(response.Content);
    }
}