const express = require('express');
const axios = require('axios');
const path = require('path');
const app = express();
const uuid = require('uuid');
const ngrok = require('ngrok');
const bodyParser = require('body-parser');  
const NodeCache = require('node-cache');
const memoryStore = new NodeCache();
const { createProxyMiddleware } = require('http-proxy-middleware');
const PORT = process.env.PORT || 10000;
const HOST_URL = process.env.REACT_APP_ISSUER_HOST_URL || 'http://bcovrintest-agent-admin-martin2.apps.exp.lab.pocquebec.org';
const urlencodedParser = bodyParser.urlencoded({ extended: false });

let schemaName      = process.env.REACT_APP_SCHEMA_NAME;    // schema name    : "vaccine";
let version         = process.env.REACT_APP_SCHEMA_VERSION; // schema version : "1.2";
let ngrokEndpoint   = '';                                   // ngrok tunnel address 

ngrok.connect({
    proto: "http", 
    addr: "10000",
}).then(url => {
    console.log(`* ngrok tunnel opened at: ${url}`); 
    ngrokEndpoint = url; 
});

app.use(express.static(path.join(__dirname, 'build')));

app.post('/create-connectionless-proof-request', urlencodedParser, async (req, res, next) => {

    let sessionId = uuid.v4();

    let connectionlessProof = {};

    let config = { headers: { 'X-API-KEY': 'cqen-api-test' } };

    let createInvitationResponse = await axios.post( HOST_URL + '/connections/create-invitation', {}, config );

    let routingKeys = createInvitationResponse.data.invitation.routingKeys;

    let recipientKeys = createInvitationResponse.data.invitation.recipientKeys;

    let serviceEndpoint = createInvitationResponse.data.invitation.serviceEndpoint;

    serviceEndpoint = 'http://bcovrintest-agent-martin2.apps.exp.lab.pocquebec.org';

    let createRequestResponse = await axios.post(HOST_URL + '/present-proof/create-request', {
        "version": "1.0",
        "trace" : "false",
        "comment" : "Vaccination proof validation", 
        "proof_request" : {
            "name"    : "vaccine", 
            "version" : "1.2", 
            "nonce": "1234567890",
            "requested_attributes" : {
                "recipient_birthDate": {
                    "name": "recipient_birthDate",
                    "restrictions": [
                        {"schema_name": schemaName , "schema_version": version}
                    ]
                },
                "recipient_fullName": {
                    "name": "recipient_fullName",
                    "restrictions": [
                        {"schema_name": schemaName, "schema_version": version}
                    ]
                }, 
                "vaccine_dateOfVaccination": {
                    "name": "vaccine_dateOfVaccination",
                    "restrictions": [
                        {"schema_name": schemaName, "schema_version": version}
                    ]
                }, 
            }, 
            "requested_predicates" : {}
            }
        }, config);

        connectionlessProof.recipientKeys = recipientKeys;
        connectionlessProof.routingKeys = routingKeys;
        connectionlessProof.serviceEndpoint = serviceEndpoint;
        connectionlessProof.requestPresentationsAttach = createRequestResponse.data.presentation_request_dict['request_presentations~attach'];
        connectionlessProof.qrcodeData =  ngrokEndpoint + "/url/" + sessionId;
        connectionlessProof.presentation_exchange_id = createRequestResponse.data.presentation_exchange_id;
        connectionlessProof.thread_id = createRequestResponse.data.thread_id;

        console.log('----------------------------------------------------');
        console.log('data: '+ JSON.stringify(createRequestResponse.data));
        console.log('----------------------------------------------------');

        memoryStore.set( sessionId, connectionlessProof, 100000 );

    res.json({'qrcodeData': connectionlessProof.qrcodeData, 'presentation_exchange_id': connectionlessProof.presentation_exchange_id});
});


app.get('/url/:sessionId', async (req, res, next) => {
    console.log("A")
    let sessionId = req.params.sessionId;
    console.log("sessionID: " + sessionId);
    let proofRequest = memoryStore.take( sessionId );
    console.log("ProofRequest: " + proofRequest);
    let dmQueryParameter = {
        "@id": proofRequest.thread_id,
        "@type": "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/present-proof/1.0/request-presentation",
        "request_presentations~attach": proofRequest.requestPresentationsAttach,
        "comment": null,
        "~service": {
            "recipientKeys": proofRequest.recipientKeys,
            "routingKeys": proofRequest.routingKeys,
            "serviceEndpoint": proofRequest.serviceEndpoint
        }
    };
    console.log("Query parameter: " + dmQueryParameter); 

    let response = ngrokEndpoint + '/link/?d_m=' + Buffer.from(JSON.stringify(dmQueryParameter)).toString('base64');

    console.log("Response: " + response); 
    console.log('redirect: '+ response);

    res.redirect(response);
});


app.use(
    '/connections',
    createProxyMiddleware({
        target: HOST_URL,
        changeOrigin: true,
    })
);

app.use(
    '/connections/create-invitation',
    createProxyMiddleware({
        target: HOST_URL,
        changeOrigin: true,
    })
);

app.use(
    '/issue-credential',
    createProxyMiddleware({
        target: HOST_URL,
        changeOrigin: true,
    })
);

app.use(
    '/credential-definitions',
    createProxyMiddleware({
        target: HOST_URL,
        changeOrigin: true,
    })
);

app.use(
    '/present-proof',
    createProxyMiddleware({
        target: HOST_URL,
        changeOrigin: true,
    })
);

app.get('*', function (req, res) {
    res.sendFile(path.join(__dirname, 'build', 'index.html'));
});

app.listen(PORT);