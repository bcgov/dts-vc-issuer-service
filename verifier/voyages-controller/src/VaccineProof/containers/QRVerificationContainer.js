import React, { useState, useEffect }       from 'react'
import { Container,  Col }                  from 'reactstrap'
import QRProofComponent                     from '../components/QRProofComponent'
import { GET_API_SECRET }                   from '../../config/constants'
import { fetchWithTimeout }                 from '../../helpers/fetchWithTimeout'
import                                           '../../assets/styles/LoginContainer.css'


function QRVerificationContainer(props){

	const [presentation_exchange_id, setPresentationExchangeId] = useState(props.location.state.invitation.presentation_exchange_id)

	let INTERVAL    = 5000; 
	let TIMEOUT     = 3000; 

    useEffect(() => {
        getConnectionInfo()
    }, []);

	function getConnectionInfo() {
		try {
			fetchWithTimeout(`/present-proof/records/${presentation_exchange_id}`,
				{
					method: 'GET',
					headers: {
						'X-API-Key': `${GET_API_SECRET()}`,
						'Content-Type': 'application/json; charset=utf-8',
					}
				}, TIMEOUT).then((
					resp => {
						try {
							resp.json().then((data => {
								if (data.state) {
									let intervalFunction;
									if (data.state === "request_sent") {
										intervalFunction = setTimeout(getConnectionInfo, INTERVAL);
									} else {
										props.history.push('/proofDisplay', {
											connection_id                               : props.location.state.connection_id,
											ticket                                      : props.location.state.ticket,
											vaccine: {
												recipient_fullName                      : data.presentation.requested_proof.revealed_attrs.recipient_fullName.raw,
												recipient_birthDate                     : data.presentation.requested_proof.revealed_attrs.recipient_birthDate.raw,
												vaccine_dateOfVaccination               : data.presentation.requested_proof.revealed_attrs.vaccine_dateOfVaccination.raw,
											}
										});
									}
								} else {
									setTimeout(getConnectionInfo, INTERVAL)
								}
							}))
						} catch (error) {
							setTimeout(getConnectionInfo, INTERVAL)
						}
					}
				))
		} catch (error) {
			console.log(error);
			setTimeout(getConnectionInfo, INTERVAL)
		}
	}

    
    
    return(
        <div className="Root" style={{ backgroundColor: '#FCF8F7', display: "flex" }}>
			<Container >
				<Col>
					<QRProofComponent value={JSON.stringify(props.location.state)} />
				</Col>
			</Container>
		</div> 
    ); 
}

export default QRVerificationContainer; 