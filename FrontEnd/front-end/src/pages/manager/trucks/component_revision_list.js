import React from 'react'
import MenuManager from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import api from '../../../service/api'
import { formatValueWithRegex} from '../../../global/utils'


class RevisionList extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            token: "",
            revisions: [],
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Avarias"

    }

    //Remove Revision
    remove(id) {
        api.delete("/api/v1/truckBreakDown/" + id, {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        alert(`Removeu com sucesso !`);
        window.location.reload(false);
    }
    detail(id) {
        window.location.href = '/revision/' + id;
    }

    componentDidMount() {

        const response = api.get("/api/v1/truckbreakdown", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ revisions: result.data }));
    }

    render() {

        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuManager />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />
                    {/* Form Start */}
                    <div className="container-fluid pt-4 px-4">
                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                            <h1 >Manutenções</h1>
                        </div>
                        <div className="col-12">
                            <div className="bg-light rounded h-100 p-4">
                                <div className="table-responsive">
                                    <table className="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Descrição</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Custo</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Camião</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Editar</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Remover</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.revisions.map((revision, index) => (
                                                <>
                                                    <tr>
                                                        <th scope="row" style={{ textAlign: 'center' }}>{index}</th>
                                                        <td style={{ textAlign: 'center' }}>{revision.date}</td>
                                                        <td style={{ textAlign: 'center' }}>{revision.description}</td>
                                                        <td style={{ textAlign: 'center' }}>{formatValueWithRegex(revision.price)}€</td>

                                                        {revision.truckReadDto != undefined &&
                                                            <>
                                                                <td style={{ textAlign: 'center' }}>{revision.truckReadDto.matricula}</td>
                                                            </>
                                                        }
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.detail(revision.id)} class="btn btn-success rounded-pill m-2">Editar</button></td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.remove(revision.id)} class="btn btn-danger rounded-pill m-2">Remover</button></td>
                                                    </tr>
                                                </>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                {/* Content End */}
            </div>
        )
    }
}



export default RevisionList

