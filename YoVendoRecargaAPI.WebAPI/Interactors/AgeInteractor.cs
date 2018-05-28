using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class AgeInteractor : IResponse
    {
        AgeBL ageBL = new AgeBL();

        public AgeResponse CreateAgeResponse(List<AgeEN> pAges)
        {
            AgeResponse response = new AgeResponse();
            Ages ages = new Ages();
            ages.agesListModel = new List<AgesListModel>();

            foreach (var agesListModel in pAges)
            {
                AgesListModel _agesListModel = new AgesListModel();

                _agesListModel.ageID = agesListModel.AgeID;
                _agesListModel.name = agesListModel.Name;
                _agesListModel.status = agesListModel.Status;
                _agesListModel.iconImage = agesListModel.IconImage;
                _agesListModel.mainImage = agesListModel.MainImage;

                ages.agesListModel.Add(_agesListModel);
            }

            response.ages = ages;
            response.count = ages.agesListModel.Count;

            return response;
        }

        public ChangeAgeResponse AgeImagesResponse(ChangeAgeEN pChangeAge, List<AgeImagesResponseEN> listImages)
        {

            List<AgeImagesCollectionResponse> _collectionImages = new List<AgeImagesCollectionResponse>();
            AgeImagesCollectionResponse _ageResponse = new AgeImagesCollectionResponse();

            ChangeAgeResponse changeAgeRes = new ChangeAgeResponse();

            changeAgeRes.AgeID = pChangeAge.AgeID;
            changeAgeRes.Name = pChangeAge.Name;
            changeAgeRes.IconImage = pChangeAge.IconImage;
            changeAgeRes.MarkerG = (from ci in listImages
                                   where ci.SequenceID == 3 & ci.Name == "Marcadores"
                                   select ci.ImgUrl).FirstOrDefault();
            changeAgeRes.MarkerS = (from ci in listImages
                                    where ci.SequenceID == 2 && ci.Name == "Marcadores"
                                   select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.MarkerB = (from ci in listImages
                                    where ci.SequenceID == 1 && ci.Name == "Marcadores"
                                   select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.MarkerW = (from ci in listImages
                                    where ci.SequenceID == 4 && ci.Name == "Marcadores"
                                   select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.WildcardMain = (from ci in listImages
                                         where ci.SequenceID == 1 && ci.Name == "Wildcard"
                                    select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.WildcardWin = (from ci in listImages
                                        where ci.SequenceID == 2 && ci.Name == "Wildcard"
                                         select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.WildcardLose = (from ci in listImages
                                         where ci.SequenceID == 3 && ci.Name == "Wildcard"
                                         select ci.ImgUrl).FirstOrDefault();

            changeAgeRes.PrizeImage = (from ci in listImages
                                         where ci.Name == "Premios"
                                         select ci.ImgUrl).FirstOrDefault();

            return changeAgeRes;
        }

    }
}