import React              from 'react';
import { Button }         from 'reactstrap';
import { useHistory }     from 'react-router-dom';
import bannerImg          from '../assets/images/banner-img-1.png';
import { useTranslation } from 'react-i18next'
import                         '../assets/styles/JumbotronComponent.css';


const LandingPageComponent = () => {
  const history = useHistory();
  const { t } = useTranslation(['translation','vaccine']);
  
  function createInvitation(){
    fetch('/create-connectionless-proof-request',{ 
        method: 'POST',
        headers: {
            'Content-Type'                 : 'application/json; charset=utf-8',
            'X-API-Key'                    : 'cqen-api-test',
            'Access-Control-Allow-Origin'  : '*', 
            'Access-Control-Allow-Methods' : 'GET, POST, PUT, PATCH, POST, DELETE, OPTIONS', 
            'Access-Control-Allow-Headers' : 'Content-Type', 
            'Access-Control-Max-Age'       : '86400'
        }
        }).then((
        resp => resp.json().then((
            data => 
            history.push('/qrproof',
                {
                type: "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/connections/1.0/invitation", 
                invitation    : data
                } 
            )
        ))
    ))
  }

  
  return (
    <header>
      <div className="pt-5 container-fluid text-center" >
        <div className="row" >
          <div className="col-md-7 col-sm-12" style={{ 'paddingTop': '0' }}>
            <h1 className="mt-5" style={{ paddingBottom: "40px"}}>{t('vaccine:travelCarefreeTitle')}</h1>
            
            <p className="sublead"> {t('vaccine:travelCarefreeDescription')} </p>
            
            <p className="sublead">{t('vaccine:directions1')}</p>

            <ol className="sublead">
              <li>{t('vaccine:directions2')}</li>
              <li>{t('vaccine:directions3')}</li>
              <li>{t('vaccine:directions4')}</li>
              <li>{t('vaccine:directions5')}</li>
              <li>{t('vaccine:directions6')}</li>
            </ol>

            <Button className="mt-5" size="lg" color="primary" onClick={() => {
                createInvitation();
            }}>{t('vaccine:tryOnBookingButtonLabel')}</Button>
            
          </div>

          <div className="col-md-4 col-sm-12">
            <img src={bannerImg} alt="CovidPerson" />
          </div>
        </div>
      </div>
    </header>
  );
};

export default LandingPageComponent;