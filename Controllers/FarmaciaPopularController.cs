using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;

using ServiceReference;

namespace consome_popular.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FarmaciaPopularController : ControllerBase
  {
    // GET api/FarmaciaPopular
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      var serviceUrl = "https://farmaciapopular-autorizador.saude.gov.br/farmaciapopular-autorizador/services/ServicoSolicitacaoWS";
      var binding = new BasicHttpBinding();
      var endpoint = new EndpointAddress(new Uri(serviceUrl));
      ChannelFactory<ServicoSolicitacaoWS> factory = new ChannelFactory<ServicoSolicitacaoWS>(binding, endpoint);
      ServicoSolicitacaoWS channel = factory.CreateChannel();
      Task.WaitAll(channel.executarEstornoAsync(null, null));


      return new string[] { "value1", "value3" };
    }
  }
}