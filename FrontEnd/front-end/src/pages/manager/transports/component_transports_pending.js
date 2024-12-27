import React from 'react'
import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import api from '../../../service/api'
import { formatValueWithRegex, formatValueIntWithRegex } from '../../../global/utils'


class TransportPending extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            transports: []
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Transportes"

    }
    //Reload Method
    reload = async e => {

        const response2 = api.get('/transportsfirebase', {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => window.location.reload(false));

    };

    loadPage(id) {
        window.location.href = '/transport/' + id;
    }

    loadPageAuto(id) {
        window.location.href = '/transportauto/' + id;
    }

    componentDidMount() {
        const response = api.get("/api/v1/transport/pending", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ transports: result.data }));
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
                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                            <h1 >Transportes Pendentes </h1>
                        </div>
                        <div className="col-12">
                            <div className="bg-light rounded h-100 p-4">
                                <div className="table-responsive">
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Cliente</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Valor</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Capacidade</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20, maxWidth: 50 }}>Origem</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Destino</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Tipo</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Analisar</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.transports.map((transport) => (
                                                <>
                                                    <tr>
                                                        <td style={{ textAlign: 'center' }}>{transport.date}</td>
                                                        {transport.client_id != undefined &&
                                                            <>
                                                                <td style={{ textAlign: 'center' }}>{transport.client_id.name}</td>
                                                            </>
                                                        }
                                                        <td style={{ textAlign: 'center' ,fontSize: 16}}>{formatValueWithRegex(transport.value_offered)} €</td>
                                                        <td style={{ textAlign: 'center',fontSize: 16 }}>{formatValueIntWithRegex(transport.capacity)}</td>
                                                        <td style={{ textAlign: 'center',fontSize: 15 }}>{transport.origin}</td>
                                                        <td style={{ textAlign: 'center' ,fontSize: 16}}>{transport.destiny}</td>
                                                        <td style={{ textAlign: 'center',fontSize: 16  }}>{transport.truckCategory}</td>
                                                        <td style={{ textAlign: 'center' ,fontSize: 16 }}><button style={{ border: 'none', background: 'none' }}><img src="/img/lupa.png" style={{ height: '30px' }} onClick={() => this.loadPage(transport.id)} /></button></td>
                                                    </tr>


                                                </>
                                            ))}

                                        </tbody>

                                    </table>
                                    <button type="button" onClick={this.reload} class="btn btn-primary rounded-pill m-2">Carregar Atualizações</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}


export default TransportPending

