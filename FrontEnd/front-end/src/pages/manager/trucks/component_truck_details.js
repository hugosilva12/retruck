import React from 'react';
import MenuManager from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import '../css/list_truck.css'
import api from '../../../service/api'
import { formatValueWithRegex} from '../../../global/utils'
import { formatValueIntWithRegex} from '../../../global/utils'

class TruckDetails extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            truck: {},
            revisions: [],
            totalCost: 0,
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }

    //Remove Revision
    remove(id) {
        api.delete("/api/v1/truck/" + id, {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        alert(`Removeu com camião com sucesso !`);
        window.location.href = '/truck/'
    }
    
   
    sum(value) {
        this.state.totalCost += value;
    }

    componentDidMount() {

        const response = api.get("/api/v1/truckbreakdown", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ revisions: result.data }));

        const response2 = api.get("/api/v1/truck/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ truck: result.data }));
    }

    render() {
        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuManager />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />
                    <div class="col-12">
                        <article class="card car-details label-info sponsored">
                            <div class="card-body">
                                <img src={this.state.truck.photoPath} style={{ maxWidth: 600, maxHeight: 300 }} />
                                <div class="d-flex flex-md-row align-items-md-start align-items-center flex-column">
                                    <div class="w-100">
                                        <div class="d-flex flex-md-row flex-column" style={{ marginTop: 30, marginLeft: 30 }}>
                                            <h3 className="text-primary">Camião - {this.state.truck.matricula}</h3>
                                        </div>
                                        <hr />
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-4 col-12">
                                                    <ul >
                                                        <li style={{ listStyle: 'none' }}><i class="fas fa-tachometer-alt"></i> <strong>kms:</strong> {formatValueIntWithRegex(this.state.truck.kms)} kms </li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-tachometer-alt"></i> <strong>Próxima Revisão:</strong> {formatValueIntWithRegex(this.state.truck.nextRevision)} kms </li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-calendar"></i> <strong>Ano:</strong> {this.state.truck.year}</li>

                                                        {this.state.truck.driver != undefined &&
                                                            <>
                                                                <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-user"></i> <strong>Condutor:</strong> {this.state.truck.driver.name}</li>
                                                            </>
                                                        }
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>Categoria:</strong> {this.state.truck.truckCategory}</li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>Perfomance:</strong> 160 hp</li>
                                                    </ul>
                                                </div>
                                                <div class="col-md-8 col-12">
                                                    <ul>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>Fuel Type:</strong> Diesel </li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>Capacidade Máxima</strong> {this.state.truck.capacity} </li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>Consumption(combined):</strong> approx. {this.state.truck.fuelConsumption} l/100km</li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><i class="fas fa-info-circle"></i> <strong>CO₂ Emissions:</strong> approx. 150 g/km (combined)</li>
                                                        <li style={{ listStyle: 'none', marginTop: 13 }}><button type="button" onClick={() => this.remove(window.location.href.split('/')[4])} class="btn btn-danger rounded-pill m-2">Remover Camião da Organização</button></li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-md-row flex-column" style={{ marginTop: 30, marginLeft: 30 }}>
                                            <h3 className="text-primary">Histórico de Revisões/Avarias</h3>
                                        </div>
                                        <hr />
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-12 col-12">
                                                    <table className="table">
                                                        <thead>
                                                            <tr>
                                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data</th>
                                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Descrição</th>
                                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Custo</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            {this.state.revisions.map((revision, index) => (
                                                                <>
                                                                    {revision.truckReadDto != undefined &&
                                                                        <>
                                                                            {revision.truckReadDto.matricula == this.state.truck.matricula &&
                                                                                <>
                                                                                    {this.sum(revision.price)}
                                                                                    <tr>
                                                                                        <th scope="row" style={{ textAlign: 'center' }}>{index}</th>
                                                                                        <td style={{ textAlign: 'center' }}>{revision.date}</td>
                                                                                        <td style={{ textAlign: 'center' }}>{revision.description}</td>
                                                                                        <td style={{ textAlign: 'center' }}>{formatValueWithRegex(revision.price)}€</td>

                                                                                    </tr>
                                                                                </>
                                                                            }
                                                                        </>
                                                                    }
                                                                </>
                                                            ))}

                                                        </tbody>
                                                    </table>
                                                    <div class="d-flex flex-md-row flex-column" style={{ marginTop: 30, marginLeft: 30 }}>
                                                        <h5 className="text">Custo total : {formatValueWithRegex(this.state.totalCost)} €</h5>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </article>
                    </div>
                </div>
            </div>
        )
    }
}



export default TruckDetails

