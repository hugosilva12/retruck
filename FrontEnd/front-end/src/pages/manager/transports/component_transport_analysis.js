import React from 'react';

import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import '../css/truck_details.css'
import api from '../../../service/api'
import { formatValueWithRegex, formatValueIntWithRegex } from '../../../global/utils'

class TransportAnalysis extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            review: {},
            truckId: "",
            kmsInitial: 0.0,
            profit: 0.0,
            showRealTime: false,
            showTrucks: [],
            countForCLose: 0,
            valueSelect: "",
            msg: ""

        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }
    showBotton() {
        this.setState({ showRealTime: !this.state.showRealTime })
    }

    async rejectTransport() {
        const response = await api.put("/api/v1/transport/" + window.location.href.split('/')[4], JSON.stringify({
        }), {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        alert("Transporte rejeitado com sucesso!");
        window.location.href = '/transportpending';


    }

    postService(truckId, availableCapacity) {

        if (this.state.countForCLose === 1) {

            const response = api.post("/api/v1/service", JSON.stringify({
                truckId: truckId,
                kms: this.state.review.kms,
                transportId: window.location.href.split('/')[4],
                availableCapacity: availableCapacity,
                profit: this.state.review.profit

            }), {
                headers: {
                    Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            alert("Transporte aceite com sucesso!");
            window.location.href = '/transportpending';

        } else {
            this.state.countForCLose = this.state.countForCLose - 1;
            const response = api.post("/api/v1/service", JSON.stringify({
                truckId: truckId,
                kms: this.state.review.kms,
                transportId: window.location.href.split('/')[4],
                availableCapacity: availableCapacity,
                profit: this.state.review.profit

            }), {
                headers: {
                    Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
        }

    }

    updateService(idService) {

        const response = api.put("/api/v1/service/" + idService, JSON.stringify({
            transportId: window.location.href.split('/')[4],
            profit: this.state.review.profit

        }), {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        response.then(result => alert("Transporte aceitado com sucesso!"), window.location.href = '/transportpending');

    }


    componentDidMount() {

        const response2 = api.get("api/v1/review/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ review: result.data }));

        /*const response3 = api.get("api/v1/review/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response3.then(result => console.log("RES ", result.data));*/

        const response = api.get("/api/v1/config", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ valueSelect: result.data.typeAnalysis }));

        const response6 = api.get("/api/v1/config", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response6.then(result => console.log("Res ", result.data));


        const response4 = api.post("/api/v1/serviceinprogress", JSON.stringify({
            id: window.location.href.split('/')[4],
        }), {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        response4.then(result => this.setState({ showTrucks: result.data, msg: "." }));

    }
    //Work Reponses

    workReview(description) {
        const strings = description.toString().split("||")
        if (strings[0] == "" || strings[0] == " " || strings[0] == undefined) {
            return "Ocupação -";
        }
        return strings[0];
    }

    updateState(valueList) {
        this.state.countForCLose = valueList;

    }

    workReview2(description) {
        const strings = description.toString().split("||")

        if (strings[1] == "" || strings[1] == " " || strings[1] == undefined)
            return "Sem análise";

        if (this.state.valueSelect == "OCCUPANCY_RATE")
            return "Sem análise";

        return strings[1];
    }

    calculateValueSpend() {
        var value = parseFloat(this.state.review.valueOffered - this.state.review.profit).toFixed(2);
        value = formatValueWithRegex(value);
        return value;
    }


    render() {
        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuSuperAdmin />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />

                    <div class="col-12">
                        <article class="card car-details label-info sponsored">
                            <div class="card-body">
                                <img src="/img/review.png" style={{ height: '300px' }} />
                                <div class="d-flex flex-md-row align-items-md-start align-items-center flex-column">
                                    <div class="w-100">
                                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                            <h1> Análise de Transporte</h1>
                                        </div>
                                        <hr />
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-4 col-12">
                                                    <ul>
                                                        <li><i class="fas fa-tachometer-alt"></i> <strong>Total kms a percorrer:</strong> {this.state.review.kms} KMs </li>
                                                        <li><i class="fas fa-calendar"></i> <strong>Data:</strong> {this.state.review.date}</li>
                                                        <li><i class="fas fa-info-circle"></i> <strong>Categoria:</strong> {this.state.review.truckCategory}</li>
                                                        <li><i class="fas fa-info-circle"></i> <strong>Origem:</strong> {this.state.review.origin}</li>
                                                        <li><i class="fas fa-info-circle"></i> <strong>Destino:</strong> {this.state.review.destiny}</li>

                                                        {this.state.review.capacity != 0 &&
                                                            <>
                                                                {this.state.review.liters == 0 &&
                                                                    <>
                                                                        <li><i class="fas fa-info-circle"></i> <strong>Volume:</strong> {formatValueIntWithRegex(this.state.review.capacity)} m3</li>
                                                                    </>
                                                                }

                                                            </>
                                                        }
                                                        {this.state.review.liters != 0 &&
                                                            <>
                                                                <li><i class="fas fa-info-circle"></i> <strong>Litros:</strong> {formatValueIntWithRegex(this.state.review.liters)}</li>
                                                            </>
                                                        }

                                                        {this.state.review.weight != 0 &&
                                                            <>
                                                                <li><i class="fas fa-info-circle"></i> <strong>Peso:</strong> {this.state.review.weight}</li>
                                                            </>
                                                        }
                                                        {this.state.review.client != undefined &&
                                                            <>
                                                                <li><i class="fas fa-info-circle"></i> <strong>Cliente:</strong> {this.state.review.client.name}</li>
                                                            </>
                                                        }

                                                        <li><i class="fa  fa-info-circle"></i> <strong>Valor Oferecido:</strong> {formatValueWithRegex(this.state.review.valueOffered)}€</li>
                                                        <li> <button type="button" style={{ backgroundColor: '#191C24' }} onClick={() => this.rejectTransport()} class="btn btn-primary rounded-pill m-2">Rejeitar Serviço</button></li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="container-fluid pt-4 px-4">
                                <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                    <h1> Review</h1>
                                </div>
                                <table className="table">
                                    <thead>
                                        <tr>

                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Gasto Previsto</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Previsão Lucro</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Possível Realizar</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estado da Análise </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>

                                            <td style={{ textAlign: 'center' }}>{this.calculateValueSpend()}€</td>
                                            <td style={{ textAlign: 'center' }}>{formatValueWithRegex(this.state.review.profit)}€</td>

                                            {this.state.review.available == true &&
                                                <>
                                                    <td style={{ textAlign: 'center' }}> Sim </td>
                                                </>
                                            }
                                            {this.state.review.available == false &&
                                                <>
                                                    <td style={{ textAlign: 'center' }}> Não</td>

                                                </>
                                            }
                                            {this.state.review.alertManagerForGoodHistory == false &&
                                                <>
                                                    <td style={{ textAlign: 'center' }}>Terminada</td>

                                                </>
                                            }
                                            {this.state.review.alertManagerForGoodHistory == true &&
                                                <>
                                                    <td style={{ textAlign: 'center' }}> Aguarda Decisão</td>

                                                </>
                                            }
                                        </tr>
                                    </tbody>

                                </table>
                            </div>
                            <br></br>
                            <div className="container-fluid pt-4 px-4">
                                <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                    <h1>Resultado da Análise</h1>
                                    <h5>({this.state.valueSelect}) </h5>
                                </div>
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Top </th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Mátricula </th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estimativa de Custo</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Análise Ocupação</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Análise Custo</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Pontuação</th>
                                            <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estado</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {this.state.review.listTrucks != null &&
                                            <>
                                                {this.state.review.listTrucks.map((transport, index) => (
                                                    <>
                                                        <tr>
                                                            <td style={{ textAlign: 'center' }}>{index}</td>
                                                            <td style={{ textAlign: 'center' }}>{transport.matricula}</td>
                                                            <td style={{ textAlign: 'center' }}>{transport.valueSpend} €</td>
                                                            <td style={{ textAlign: 'center' }}>{this.workReview(transport.summaryReview)} {"  " + transport.occupiedVolumePercentage + "%"} </td>
                                                            <td style={{ textAlign: 'center' }}>{this.workReview2(transport.summaryReview)}</td>
                                                            <td style={{ textAlign: 'center' }}>{transport.score}</td>
                                                            {transport.isAvailable == true &&
                                                                <>
                                                                    <td style={{ textAlign: 'center' }}><img src="/img/icon.png" style={{ height: '27px' }} /></td>

                                                                </>
                                                            }
                                                            {transport.isAvailable == false &&
                                                                <>
                                                                    <td style={{ textAlign: 'center' }}><img src="/img/iconred.png" style={{ height: '27px' }} /></td>

                                                                </>
                                                            }
                                                        </tr>
                                                    </>
                                                ))}
                                            </>
                                        }
                                    </tbody>

                                </table>
                            </div>
                            <br></br>
                            {this.state.review.thinksTwice == true &&
                                <>
                                    {this.state.review.alertManagerForGoodHistory == false &&
                                        <>
                                            <div className="container-fluid pt-4 px-4">
                                                <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                                    <h1>Atenção  <img src="/img/iconred.png" style={{ height: '40px' }} /></h1>
                                                    <h3>Serviço dá prejuizo e não existe um histórico bom histórico com este cliente.</h3>
                                                    <br></br>
                                                </div>
                                            </div>
                                        </>}
                                    <br></br>
                                </>}
                            {this.state.review.alertManagerForGoodHistory == true &&
                                <>
                                    <div className="container-fluid pt-4 px-4">
                                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                            <h1>Atenção  <img src="/img/alter.png" style={{ height: '40px' }} /></h1>
                                            <h3>Serviço dá prejuizo, no entanto existe bom histórico com este cliente. Reveja o  histórico de serviços.</h3>
                                        </div>
                                    </div>
                                    <br></br>
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>kms </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Categoria</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Lucro</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.review.listServices.map((transport) => (
                                                <>
                                                    <tr>
                                                        <td style={{ textAlign: 'center' }}>{transport.transportReadDto.date}</td>
                                                        <td style={{ textAlign: 'center' }}>{transport.kms}</td>
                                                        <td style={{ textAlign: 'center' }}>{transport.transportReadDto.truckCategory}</td>
                                                        <td style={{ textAlign: 'center' }}>{formatValueWithRegex(transport.profit)}€ </td>
                                                        {transport.profit > 0 &&
                                                            <>
                                                                <td style={{ textAlign: 'center' }}><img src="/img/icon.png" style={{ height: '27px' }} /></td>

                                                            </>
                                                        }
                                                        {transport.profit < 0 &&
                                                            <>
                                                                <td style={{ textAlign: 'center' }}> <img src="/img/iconred.png" style={{ height: '27px' }} /></td>

                                                            </>
                                                        }
                                                        {transport.profit == 0 &&
                                                            <>
                                                                <td style={{ textAlign: 'center' }}><img src="/img/null.png" style={{ height: '27px' }} /></td>

                                                            </>
                                                        }
                                                    </tr>

                                                </>
                                            ))}

                                        </tbody>
                                    </table>

                                    <br></br>
                                    <br></br>
                                </>}
                            <div className="container-fluid pt-4 px-4">
                                <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                    <h1>Camião selecionado(s)  <img src="/img/lupa.png" style={{ height: '50px' }} /></h1>
                                </div>
                            </div>
                            <div className="container-fluid pt-4 px-4">

                                <table className="table">
                                    <thead>
                                        {this.state.review.description == 'None' &&
                                            <>
                                                <tr>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Mátricula </th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Condutor </th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Combustivel Gasto </th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Ação</th>
                                                </tr>
                                            </>}
                                    </thead>
                                    <tbody>
                                        {this.state.review.truckSelected != null &&
                                            <>
                                                {this.updateState(this.state.review.truckSelected.length)}
                                                {this.state.review.description == 'None' &&
                                                    <>
                                                        {this.state.review.truckSelected.map((transport, index) => (
                                                            <>
                                                                <tr>
                                                                    <td style={{ textAlign: 'center' }}>{transport.matricula}</td>
                                                                    <td style={{ textAlign: 'center' }}>{transport.driver.name}</td>
                                                                    <td style={{ textAlign: 'center' }}>{formatValueWithRegex(transport.litresSpend)} litros</td>
                                                                    <td style={{ textAlign: 'center' }}>   <button type="button" onClick={() => this.postService(transport.id, transport.availableCapacity)} class="btn btn-success rounded-pill m-2">Atribuir</button></td>
                                                                </tr>

                                                            </>
                                                        ))}
                                                    </>}
                                            </>
                                        }
                                        {this.state.review.description != 'None' &&
                                            <>
                                                <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                                    <h3>{this.state.review.description}</h3>
                                                </div>
                                            </>}


                                    </tbody>

                                </table>

                            </div>
                            <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center', marginTop: 20 }}>
                                <button type="button" style={{ backgroundColor: '#191C24' }} onClick={() => this.showBotton()} class="btn btn-primary rounded-pill m-2">Ver camiões em serviço  perto da Zona{this.state.msg}</button>

                            </div>

                            {this.state.showRealTime === true &&
                                <>
                                    <div className="container-fluid pt-4 px-4">
                                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                            <h1> Camiões em Serviço perto da Zona</h1>
                                        </div>
                                        <table className="table">
                                            <thead>
                                                <tr>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Mátricula </th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Localização Atual </th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Distância</th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Destino</th>
                                                    <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Ação</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {this.state.showTrucks != [] &&
                                                    <>
                                                        {this.state.showTrucks.map((transport) => (
                                                            <>
                                                                <tr>
                                                                    {transport.truckReadDto != undefined &&
                                                                        <>
                                                                            <td style={{ textAlign: 'center' }}>{transport.truckReadDto.matricula}</td>
                                                                        </>}
                                                                    <td style={{ textAlign: 'center' }}>{transport.currentLocation}</td>
                                                                    <td style={{ textAlign: 'center' }}>{transport.kms} kms</td>
                                                                    <td style={{ textAlign: 'center' }}>{transport.transportReadDto.destiny}</td>
                                                                    <td style={{ textAlign: 'center' }}>   <button type="button" onClick={() => this.updateService(transport.id)} class="btn btn-success rounded-pill m-2">Atribuir</button></td>
                                                                </tr>

                                                            </>
                                                        ))}
                                                    </>}
                                            </tbody>
                                        </table>
                                    </div>
                                </>
                            }

                        </article>

                    </div>

                </div>

            </div>
        )
    }
}



export default TransportAnalysis

