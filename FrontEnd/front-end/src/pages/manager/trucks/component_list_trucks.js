import React from 'react';
import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import '../css/list_truck.css'
import api from '../../../service/api'


class ListTruck extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            trucks: []
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }

    componentDidMount() {
        const response2 = api.get("/api/v1/truck", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ trucks: result.data }));
    }


    render() {

        return (
            <div className="container-xxl position-relative bg-white d-flex p-0">
                <MenuSuperAdmin />
                {/* Content Start */}
                <div className="content">
                    <Toolbar />
                    <div className="container-fluid pt-4 px-4">
                        <div class="row">
                            {this.state.trucks.map((driver) => (
                                <>
                                    <div class="col-md-4 " style={{ marginTop: 15 }} >
                                        <div class="card">
                                            <div class="card-img ">
                                                <img class="img-responsive" src={driver.photoPath} style={{ width: '100%' }} />
                                            </div>
                                            <div class="card-body">
                                                <div class="card-title">
                                                    <h4 className="text-primary"> <a href={"/truck/" + driver.id}>{driver.matricula} </a></h4>
                                                    <ul class="list-inline ">
                                                        <li class="list-inline-item"><i class="fa fa-truck"></i>  Ano- {driver.year}</li>
                                                        <li class="list-inline-item"><i class="fa fa-tint"></i> Diesel</li>
                                                    </ul>
                                                    <ul class="list-inline ">
                                                    <li class="list-inline-item"><i class="fa fa-list-alt"></i> {driver.capacity} </li>
                                                    <li class="list-inline-item"><i class="fa fa-list-alt"></i> {driver.truckCategory} </li>
                                                    </ul>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </>
                            ))}
                        </div>
                    </div>
                </div>
                {/* Content End */}
            </div>
        )
    }
}



export default ListTruck

