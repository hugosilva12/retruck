import React from 'react';

import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import '../css/truck_details.css'
import api from '../../../service/api'
import MapComponentService from '../../../components/utils/map'
import { formatValueWithRegex } from '../../../global/utils';
class ServicesDetails extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            token: "",
            locations: [],
            serviceDetails: {},
          
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Serviço"

    }

    
    reload = async e => {

        const response2 = api.get('/api/v1/service/updateStateService', {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        const response3 = api.get('/location_services', {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response3.then(result => window.location.reload(false));

    };


    componentDidMount() {

        const response2 = api.get("/api/v1/service/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ serviceDetails: result.data }));


        const response3 = api.get("/api/v1/service/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response3.then(result => console.log("Result ", result.data));

      
    }


    render() {

        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuSuperAdmin />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />

                    {/* Form Start */}
                    <div className="container-fluid pt-4 px-4">
                        <div className="row g-4">
                            <div className="col-6">
                                <div className="bg-light rounded h-100 p-4">
                                    <div className="table-responsive">
                                        <table className="table text-start align-middle table-bordered table-hover mb-0">
                                            <tbody>

                                                {this.state.serviceDetails.truckReadDto != undefined &&
                                                    <>
                                                        {this.state.serviceDetails.truckReadDto.driver != undefined &&
                                                            <>
                                                                <td className="text-dark" style={{ textAlign: 'center' }}>Condutor</td>
                                                                <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.truckReadDto.driver.username}</td>
                                                            </>}
                                                        <tr>

                                                        </tr>
                                                    </>}
                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Estado</td>
                                                    <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.status}</td>
                                                </tr>
                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Lucro esperado</td>
                                                    <td style={{ textAlign: 'center' }}>{formatValueWithRegex(this.state.serviceDetails.profit)}€</td>

                                                </tr>
                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Localização Atual</td>
                                                    <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.currentLocation}</td>
                                                </tr>
                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Estimativa de chegada</td>
                                                    <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.durationToFinish}</td>

                                                </tr>
                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Capacidade Restante</td>
                                                    <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.capacityAvailable}</td>

                                                </tr>
                                                {this.state.serviceDetails.transportReadDto != undefined &&
                                                    <>  <tr>
                                                        <td className="text-dark" style={{ textAlign: 'center' }}>Categoria </td>
                                                        <td style={{ textAlign: 'center' }}>{this.state.serviceDetails.transportReadDto.truckCategory}</td>
                                                    </tr>
                                                    </>}

                                                <tr>
                                                    <td className="text-dark" style={{ textAlign: 'center' }}>Atualizar Estado</td>
                                                    <td style={{ textAlign: 'center' }}> <button type="button" onClick={this.reload} class="btn btn-primary rounded-pill m-2">Atualizar</button>  </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div className="col-sm-12 col-xl-12">

                            {this.state.serviceDetails.listCoord != undefined &&
                                <>
                                    {this.state.serviceDetails.listCoord != null &&
                                        <>
                                            <MapComponentService name="Hugo" organizationLocation={this.state.serviceDetails.organizationAddressCoord} nowLocationTruck={this.state.serviceDetails.nowLocationTruck} initServiceAddress={this.state.serviceDetails.initServiceAddress} finishService={this.state.serviceDetails.finishService} locations={this.state.serviceDetails.listCoord} />
                                        </>
                                    }
                                </>
                            }
                  

                    </div>
                </div>
            </div>
                {/* Content End */ }
            </div >
        )
    }
}


export default ServicesDetails

