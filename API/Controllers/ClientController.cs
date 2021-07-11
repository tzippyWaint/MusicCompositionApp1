using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    
        [RoutePrefix("api/client")]
        public class ClientController : ApiController
        {
            MusicCompositionBL.classes.ClientsBL clientsBL;
            MusicCompositionBL.classes.PlayersBL playersBL;

            [AcceptVerbs("GET", "POST")]
            [Route("signup")]
            [HttpPost]
            public int SignUp(Clients clients)
            {
                clientsBL = new MusicCompositionBL.classes.ClientsBL();
                return clientsBL.InsertClient(clients);
            }
        [Route("login/{id}")]
        [HttpGet]
        public int[] Login(int id)
        {
            //213626922
            //213520463
            int[] answer = new int[2];
            clientsBL = new MusicCompositionBL.classes.ClientsBL();
            playersBL = new MusicCompositionBL.classes.PlayersBL();
            if (clientsBL.listOfClients.Find(c => c.idC == id) != null)
            {
                answer[0] = clientsBL.listOfClients.Find(c => c.idC == id).codeCli;
                answer[1] = 1;
            }
            if (playersBL.listOfPlayers.Find(p => p.idP == id) != null)
            {
                answer[0] = playersBL.listOfPlayers.Find(p => p.idP == id).codeP;
                //player
                if (playersBL.listOfPlayers.Find(p => p.idP == id).status == "active")
                    answer[1] = 2;
                //conductor
                else
                    if (playersBL.listOfPlayers.Find(p => p.idP == id).status == "activeC")
                    answer[1] = 3;     
            }
            //manager
            if (id== 217240415)
            {
                answer[0] = 412;
                answer[1] = 4;
            }
                
            return answer;
        }
        [Route("updateclient")]
        [HttpPost]
        public int UpDateClient(Clients clients)
        {
            //213626922;
            //= new Clients() {idC = 12345678, fullNameC = "gcc", pel1 = "055678989", pel2 = "45678", email = "fghj", points = 3, status = "active" };
            clientsBL = new MusicCompositionBL.classes.ClientsBL();
            return clientsBL.UpDateClient(clients);           
        }
        [Route("deleteclient")]
        [HttpPost]
        public int DeleteClient(Clients clients)
        {
             //= new Clients() { idC = 12345678, fullNameC = "ghj", pel1 = "055678989", pel2 = "45678", email = "fghj", points = 3, status = "active" };
            clientsBL = new MusicCompositionBL.classes.ClientsBL();
            return clientsBL.DeleteClient(clients);
        }
        [Route("show")]
        [HttpGet]
        public List<ClientsModel> Show()
        {
            clientsBL = new MusicCompositionBL.classes.ClientsBL();
            return clientsBL.ConvertListToModel(clientsBL.listOfClients);
        }
    }
    
}
