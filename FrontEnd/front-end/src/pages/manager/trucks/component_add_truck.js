import React from 'react';
import MenuManager from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import Footer from '../../../components/utils/footer'
import api from '../../../service/api'
import { TruckCategory } from '../../../global/truck_category'

class AddTruck extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            value: 0,
            token: "",
            matricula: "",
            profile: -1,
            organization_id: "",
            organizations: [],
            error: "",
            license: -1,
            showAlert: false,
            showLicence: false,
            photofilename: "",
            driver_id: "",
            imagesrc: process.env.REACT_APP_PHOTOPATH + "anonymous.png",
            drivers: [],
            truckNotRegistered: []
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }

    photofilename = "anonymous.png";
    imagesrc = process.env.REACT_APP_PHOTOPATH + this.photofilename;

    //Selects
    handleChangeProfile = (e) => {
        this.setState({ profile: e.target.value });
        if (e.target.value == 0) {
            this.setState({ showLicence: true });
        } else {
            this.setState({ showLicence: false });
        }
    }
    handleChangeLicense = (e) => {
        this.setState({ license: e.target.value });
        this.setState({ showLicence: true });

    }

    handleMatricula = (e) => {
        console.log("Matricula ", e.target.value)
        this.setState({ matricula: e.target.value })
    }

    handleChangeOrganization = (e) => {
        this.setState({ organization_id: e.target.value });

    }

    handleChangeDriver = (e) => {
        this.setState({ driver_id: e.target.value });

    }

    //Upload-Photos
    handleFileSelected(event) {
        event.preventDefault();

        const formData = new FormData();
        formData.append(
            "myFile",
            event.target.files[0],
            event.target.files[0].name
        );

        fetch('https://localhost:7226/api/v1/photo/save-filesTruck', {
            method: 'POST',
            body: formData
        })
    }

    componentDidMount() {

        const response = api.get("/api/v1/organization", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response.then(result => this.setState({ organizations: result.data }));

        const response2 = api.get("/api/v1/user/driverswithlicence", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ drivers: result.data }));

        const response3 = api.get("/api/v1/truck/trucksnotregistration", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response3.then(result => this.setState({ truckNotRegistered: result.data }));


    }

    handlePost = async e => {
        e.preventDefault();
        let isToPost = true
        const { matricula, license, organization_id, driver_id } = this.state;
        if (organization_id == "") {
            this.setState({ error: "Atribua Organização!" });
            isToPost = false
        }
        if (driver_id == "") {
            this.setState({ error: "Nenhum Condutor associado" });
            isToPost = false
        }


        if (isToPost) {
            const response = await api.post("/api/v1/truck", JSON.stringify({
                matricula: matricula,
                driverId: driver_id,
                organizationId: organization_id,
                photofilename: process.env.REACT_APP_PHOTOPATH,
                truckCategory: license

            }), {
                headers: {
                    Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (response.status == 200) {
                alert(`Criado com sucesso!`);
                window.location.reload(false);
            } else {
                this.setState({ error: "Campos Inválidos" });
            }
        }
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
                            <h1 >Criar Camião</h1>
                        </div>
                        <div className="row g-4">
                            <div className="col-sm-12 col-xl-12">
                                <div className="bg-light rounded h-100 p-4">
                                    <img src="img/truck-icon.png" style={{ height: 70 }} />
                                    <form>
                                        <div className="form-floating mb-3">
                                            <select onChange={this.handleChangeLicense} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                <option selected></option>
                                                {TruckCategory.map((truck) => (
                                                    <>
                                                        <option value={truck.category}>{truck.category}</option>

                                                    </>
                                                ))}
                                            </select>
                                            <label htmlFor="floatingSelect">Categoria</label>
                                        </div>

                                        <div className="form-floating mb-3">
                                            <select onChange={this.handleMatricula} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                <option selected> </option>
                                                {this.state.truckNotRegistered.map((truck) => (
                                                    <>
                                                        {truck.truckCategory == this.state.license &&
                                                            <option value={truck.matricula}>{truck.matricula}</option>
                                                        }

                                                    </>

                                                ))}
                                            </select>
                                            <label htmlFor="floatingSelect">Matricula</label>
                                        </div>

                                        <div className="form-floating mb-3">
                                            <select onChange={this.handleChangeOrganization} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                <option selected> </option>
                                                {this.state.organizations.map((organization) => (

                                                    <option value={organization.id}>{organization.name}</option>
                                                ))}
                                            </select>
                                            <label htmlFor="floatingSelect">Organização</label>
                                        </div>

                                        {this.state.showLicence == true &&
                                            <>
                                                <div className="form-floating mb-3">
                                                    <select onChange={this.handleChangeDriver} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                                                        <option selected></option>
                                                        {this.state.drivers.map((drivers) => (
                                                            <>
                                                                {drivers.truckCategory == this.state.license &&
                                                                    <option value={drivers.id_driver}>{drivers.name}</option>
                                                                }

                                                            </>
                                                        ))}
                                                    </select>
                                                    <label htmlFor="floatingSelect">Condutor</label>
                                                </div>

                                            </>
                                        }
                                        <div className="form-floating mb-3">
                                            <input onChange={this.handleFileSelected} type="File" />
                                        </div>
                                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
                                            <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 140, height: 50 }}>Criar Camião</button>
                                        </div>
                                        <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center', marginTop: 20 }}>
                                            <h3 class="text-center mb-0">{this.state.error}</h3>
                                        </div>
                                    </form>

                                </div>
                            </div>

                        </div>
                    </div>
                    {/* Form End */}
                    {/* Footer Start */}
                    <Footer />
                    {/* Footer End */}
                </div>
                {/* Content End */}
            </div>
        )
    }
}



export default AddTruck

