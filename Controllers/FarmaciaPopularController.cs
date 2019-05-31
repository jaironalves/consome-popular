using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;
using ServiceReference;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;

namespace consome_popular.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FarmaciaPopularController : ControllerBase
  {
    private readonly IHostingEnvironment _hostingEnvironment;

    public FarmaciaPopularController(IHostingEnvironment hostingEnvironment)
    {
      _hostingEnvironment = hostingEnvironment;
    }


    // GET api/FarmaciaPopular
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      var serviceUrl = "https://farmaciapopular-autorizador.saude.gov.br/farmaciapopular-autorizador/services/ServicoSolicitacaoWS";
      var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
      binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
      var endpoint = new EndpointAddress(new Uri(serviceUrl));

      var path = System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, "Cert_Prod.DER");
      var certificate = new X509Certificate2(System.IO.File.ReadAllBytes(path));

      ChannelFactory<ServicoSolicitacaoWS> factory = new ChannelFactory<ServicoSolicitacaoWS>(binding, endpoint);
      factory.Credentials.ClientCertificate.Certificate = certificate;

      ServicoSolicitacaoWS channel = factory.CreateChannel();
      var estorno = new EstornoDTO();
      estorno.nuAutorizacao = "2020202202";

      var usuario = new UsuarioFarmaciaDTO();
      usuario.usuarioVendedor = "ass";

      var response = await channel.executarEstornoAsync(estorno, usuario);


      return Ok(new string[] { response.inSituacaoEstorno, response.descMensagemErro });
    }
  }
}