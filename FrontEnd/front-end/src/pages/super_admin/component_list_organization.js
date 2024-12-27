import React from 'react';
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import api from '../../service/api';
import Footer from '../../components/utils/footer';

class HomeListOrganization extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            organizations: [],
            managers: [],
            token: "",
        }
        this.state.token = localStorage.getItem("token");
        document.title = "Organizations"
    }


    componentDidMount() {
        const response = api.get("/api/v1/organization",{
            headers: { Authorization: `Bearer ${this.state.token}`, 'Accept':'application/json',
            'Content-Type':'application/json' }
        });
           
        response.then(result => this.setState({ organizations: result.data }));
    }

    detail(id) {
        window.location.href = '/organization/' + id;
    }
    
     //Remove Revision
     remove(id) {
        api.delete("/api/v1/organization/" + id, {
            headers: {
                Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });
        alert(`Removeu com sucesso !`);
        window.location.reload(false);
    }
    render() {
        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuSuperAdmin />
                <div className="content">
                    <Toolbar />
                    <div class="container-fluid pt-4 px-4">
                        <div class="row g-4">
                            <div class="col-sm-12 col-xl-12">
                                <div class="bg-light rounded h-100 p-4">
                                    <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                        <h1 >Organizações</h1>
                                    </div>
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th scope="col"style={{ textAlign: 'center' }}>#</th>
                                                <th scope="col"style={{ textAlign: 'center' }}>No</th>
                                                <th scope="col"style={{ textAlign: 'center' }}>Nome </th>
                                                <th scope="col"style={{ textAlign: 'center' }}>Localização</th>
                                                <th scope="col"style={{ textAlign: 'center' }}>Vatin</th>
                                                <th scope="col"style={{ textAlign: 'center' }}>Editar</th>
                                                <th scope="col"style={{ textAlign: 'center' }}>Delete</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.organizations.map((driver, index) => (
                                                <>
                                                    <tr>
                                                        <th style={{ textAlign: 'center' }}><img src="img/organization.png" style={{ height: 30 }} /></th>
                                                        <th scope="row" style={{ textAlign: 'center' }}>{index}</th>
                                                        <td style={{ textAlign: 'center' }}>{driver.name}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.addresses}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.vatin}</td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.detail(driver.id)} class="btn btn-warning rounded-pill m-2">Editar</button></td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.remove(driver.id)} class="btn btn-danger rounded-pill m-2">Remover</button></td>
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
            </div>
        )
    }
}


export default HomeListOrganization

