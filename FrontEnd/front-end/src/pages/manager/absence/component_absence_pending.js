import React from 'react'
import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import api from '../../../service/api'

class AbsencePending extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            absences: []
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Ausências"

    }
    
    //Reload Method
    reload = async e => {

        const response2 = api.get("/absencefirebase", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });
        response2.then(result => window.location.reload(false));
    };

    refuse(id) {
        api.put("/api/v1/absence/" + id, JSON.stringify({
            status: 'REJECTED',
        }), {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        alert(`Atualizou com sucesso !`);
        window.location.reload(false);
    }

    accepted(id) {
        api.put("/api/v1/absence/" + id, JSON.stringify({
            status: 'ACCEPT',
        }), {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        alert(`Atualizou com sucesso !`);
        window.location.reload(false);
    }

    componentDidMount() {

        const response = api.get("/api/v1/absence", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ absences: result.data }));

        const response2 = api.get("/api/v1/absence", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => console.log(result.data));
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
                            <h1 >Ausências Pendentes </h1>
                        </div>
                        <div className="col-12">
                            <div className="bg-light rounded h-100 p-4">
                                <div className="table-responsive">
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Funcionário</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Motivo</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Descrição</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Aceitar</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Rejeitar</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.absences.map((abscence, index) => (
                                                <>
                                                    {abscence.status == 'WAIT_APROVE' &&
                                                        <>
                                                            <tr>
                                                                <th scope="row" style={{ textAlign: 'center' }}>{index}</th>
                                                                <td style={{ textAlign: 'center' }}>{abscence.date}</td>
                                                                {abscence.driver != undefined &&
                                                                    <>
                                                                        <td style={{ textAlign: 'center' }}>{abscence.driver.name}</td>
                                                                    </>
                                                                }
                                                                <td style={{ textAlign: 'center' }}>{abscence.abscence}</td>
                                                                <td style={{ textAlign: 'center' }}>{abscence.description}</td>
                                                                <td style={{ textAlign: 'center' }}> <button type="button" onClick={() => this.accepted(abscence.id)} class="btn btn-success rounded-pill m-2">Aceitar</button></td>
                                                                <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.refuse(abscence.id)} class="btn btn-danger rounded-pill m-2">Recusar</button></td>

                                                            </tr>
                                                        </>
                                                    }

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


export default AbsencePending

