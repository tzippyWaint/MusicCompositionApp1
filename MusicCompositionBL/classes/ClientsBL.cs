using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using MusicCompositionDAL;
namespace MusicCompositionBL.classes
{
    public class ClientsBL
    {
        DBConection dbCon;
        public List<Clients> listOfClients;
        public ClientsBL()
        {
            dbCon = new DBConection();
            listOfClients = dbCon.GetDbSet<Clients>().ToList();
        }
        public List<Clients> GetAllClients()
        {
            return listOfClients;
        }
        //הוספת לקוח
        public int InsertClient(Clients clients)
        {
            if (listOfClients.Find(c => c.idC == clients.idC) == null)
                    try
                    {
                        dbCon.Execute<Clients>(new Clients() { codeCli = listOfClients.Max(c => c.codeCli)+1, idC = clients.idC, fullNameC = clients.fullNameC.ToString(), pel1 = clients.pel1, pel2 = clients.pel2, email =clients. email, points = clients.points, status =clients.status,Appearances=clients.Appearances}, DBConection.ExecuteActions.Insert);
                        listOfClients = dbCon.GetDbSet<Clients>().ToList();
                        return listOfClients.First(c => c.idC == clients.idC).codeCli;
                    }
                    catch(Exception ex)
                    {
                        return 0;
                    }
                             
            return listOfClients.First(c => c.idC == clients.idC ).codeCli; 

        }
        //מחיקת לקוח
        public int DeleteClient(Clients client)
        {
            MusicCompositionBL.classes.AppearancesBL appearancesBL = new AppearancesBL();
            if (listOfClients.Find(c => c.idC == client.idC) != null&& (appearancesBL.listOfAppearances.Find(a=>a.codeCli==client.codeCli)==null || appearancesBL.listOfAppearances.Find(a => a.codeCli == client.codeCli).dateA <= DateTime.Now))
                try
                {
                    dbCon.Execute<Clients>(listOfClients.Find(c => c.idC == client.idC), DBConection.ExecuteActions.Delete);
                    listOfClients= dbCon.GetDbSet<Clients>().ToList();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            return 2;
        }
        //עדכון לקוח
        public int UpDateClient(Clients client)
        {
            if (listOfClients.Find(c => c.idC == client.idC) != null)
                try
                {
                dbCon.Execute<Clients>(client, DBConection.ExecuteActions.Update);
                return 1;
                }
                catch
                {
                return 0;
                }
            return 2;
        }
        public Clients ConvertClientsToEF(ClientsModel c)
        {
            return new Clients
            {
                codeCli=c.codeCli,fullNameC=c.fullNameC,idC=c.idC,pel1=c.pel1,pel2=c.pel2,points=c.points,email=c.email
            };
        }
        public static ClientsModel ConvertClientsToModel(Clients c )
        {
            return new ClientsModel
            {
                codeCli = c.codeCli,
                fullNameC = c.fullNameC,
                idC = c.idC,
                pel1 = c.pel1,
                pel2 = c.pel2,
                points = c.points,
                email = c.email,
                status=c.status
            };
        }
        public List<ClientsModel> ConvertListToModel(List<Clients> i)
        {
            List<ClientsModel> l = new List<ClientsModel>();
            Clients c = new Clients();
            foreach (var item in i)
            {
                c.codeCli = item.codeCli;
                c.fullNameC = item.fullNameC;
                c.idC = item.idC;
                c.pel1 = item.pel1;
                c.pel2 = item.pel2;
                c.points = item.points;
                c.email = item.email;
                c.status = item.status;
                l.Add(ConvertClientsToModel(c));
            }
            return l;
        }
    }
}
