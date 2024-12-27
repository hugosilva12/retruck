import React from 'react';
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import api from '../../service/api';
import Footer from '../../components/utils/footer';
class HomeListEmployee extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            drivers: [],
            managers: [],
            token: "",
        }
        this.state.token = localStorage.getItem("token");
        document.title = "Employees"
    }

    componentDidMount() {
        const response = api.get("/api/v1/user", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ drivers: result.data }));

        const response2 = api.get("/api/v1/user/managers", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ managers: result.data }));

    }

    detail(id) {
        window.location.href = '/user/' + id;
    }
    

    //Remove Employee
    remove(id) {
        api.delete("/api/v1/user/" + id, {
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
                                    <h6 class="mb-4">Drivers</h6>
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>No</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Username </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Nome </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Email </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estado</th>
                                                <th scope="col"style={{ textAlign: 'center' , fontSize: 20 }}>Editar</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Ação</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.drivers.map((driver, index) => (
                                                <>
                                                    <tr>
                                                        <th><img src="img/c.png" style={{ height: 30 }} /></th>
                                                        <th scope="row">{index}</th>
                                                        <td style={{ textAlign: 'center' }}>{driver.username}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.name}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.email}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.userState}</td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.detail(driver.id)} class="btn btn-warning rounded-pill m-2">Editar</button></td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.refuse(driver.id)} class="btn btn-danger rounded-pill m-2">Eliminar</button></td>
                                                    </tr>
                                                </>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="col-sm-12 col-xl-12">
                                <div class="bg-light rounded h-100 p-4">
                                    <h6 class="mb-4">Managers</h6>
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>No</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Username </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Nome </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Email </th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estado</th>
                                                <th scope="col"style={{ textAlign: 'center' , fontSize: 20 }}>Editar</th>
                                                <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Ação</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                            {this.state.managers.map((driver, index) => (
                                                <>
                                                    <tr>
                                                        <th><img src="img/c.png" style={{ height: 30 }} /></th>
                                                        <th scope="row">{index}</th>
                                                        <td style={{ textAlign: 'center' }}>{driver.username}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.name}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.email}</td>
                                                        <td style={{ textAlign: 'center' }}>{driver.userState}</td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.detail(driver.id)} class="btn btn-warning rounded-pill m-2">Editar</button></td>
                                                        <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.remove(driver.id)} class="btn btn-danger rounded-pill m-2">Eliminar</button></td>
                                                    </tr>
                                                </>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                    <Footer />
                </div>
            </div>
        )
    }
}


export default HomeListEmployee

