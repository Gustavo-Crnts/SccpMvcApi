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
  public class JogadorController : Controller
  {
    public string uriBase = "http://sccpapi.somee.com/Jogador/";

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
          List<JogadorViewModel> listaJogadores = await Task.Run(() =>
          JsonConvert.DeserializeObject<List<JogadorViewModel>>(serialized));

          return View(listaJogadores);

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
    public async Task<ActionResult> CreateAsync(JogadorViewModel j)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(JsonConvert.SerializeObject(j));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] = string.Format("Jogador [0], Id {1} salvo com sucesso!", j.Nome, serialized);
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
          JogadorViewModel j = await Task.Run(() =>
              JsonConvert.DeserializeObject<JogadorViewModel>(serialized));
          return View(j);
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
          JogadorViewModel j = await Task.Run(() =>
              JsonConvert.DeserializeObject<JogadorViewModel>(serialized));
          return View(j);
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
    public async Task<ActionResult> EditAsync(JogadorViewModel j)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(JsonConvert.SerializeObject(j));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await httpClient.PutAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] =
                  string.Format("Jogador {0}, posição {1} atualizado com sucesso!", j.Nome, j.Posicao);

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
          TempData["Mensagem"] = string.Format("Jogador Id {0} removido com sucesso!", id);
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

