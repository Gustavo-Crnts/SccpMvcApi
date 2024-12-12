using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SccpMvcApi.Models;

namespace SccpMvcApi.Controllers
{
    public class TitulosController : Controller
    {
        
         public string uriBase = "http://sccpapi.somee.com/Titulos/";

    [HttpGet]
    public async Task<ActionResult> IndexAsync()
    {
      try
      {
        string uriComplementar = "GetAll";
        HttpClient httpClient = new HttpClient();
        //string token = HttpContext.Session.GetString("SessionTokenUsuario");
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          List<TitulosViewModel> listaTitulos = await Task.Run(() =>
          JsonConvert.DeserializeObject<List<TitulosViewModel>>(serialized));

          return View(listaTitulos);

        }
        else
          throw new System.Exception(serialized);

      }

      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }


    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(TitulosViewModel t)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(JsonConvert.SerializeObject(t));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] = string.Format("Titulo [0], Id {1} salvo com sucesso!", t.Campeonato, serialized);
          return RedirectToAction("Index");
        }

        else
          throw new System.Exception(serialized);
      }

      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Create");
      }
    }

    [HttpGet]
    public ActionResult Create()
    {
      return View();
    }

    [HttpGet]
    public async Task<ActionResult> DetailsAsync(int? id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TitulosViewModel t = await Task.Run(() =>
              JsonConvert.DeserializeObject<TitulosViewModel>(serialized));
          return View(t);
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View("Error!");
    }

    [HttpGet]
    public async Task<ActionResult> EditAsync(int? id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());

        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TitulosViewModel t = await Task.Run(() =>
              JsonConvert.DeserializeObject<TitulosViewModel>(serialized));
          return View(t);
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public async Task<ActionResult> EditAsync(TitulosViewModel t)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(JsonConvert.SerializeObject(t));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await httpClient.PutAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] =
                  string.Format("Titulo {0}, data  {1} atualizado com sucesso!", t.Campeonato, t.Data);

          return RedirectToAction("Index");
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }

    [HttpGet]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.DeleteAsync(uriBase + id.ToString());
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] = string.Format("Titulo Id {0} removido com sucesso!", id);
          return RedirectToAction("Index");
        }
        else
        {
          throw new System.Exception(serialized);
        }
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }
    }
}