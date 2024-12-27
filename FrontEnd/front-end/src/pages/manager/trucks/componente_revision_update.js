import React from 'react';
import MenuManager from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import Footer from "../../../components/utils/footer"
import api from '../../../service/api'
import Datetime from 'react-datetime'
import "react-datetime/css/react-datetime.css";
class UpdateRevision extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            price: 0.0,
            token: "",
            description: "",
            organization_id: "",
            organizations: [],
            error: "",
            photofilename: "",
            driver_id: "",
            trucks: [],
            date: "",
            olddate:"",
            revision: {}
        }

        this.state.token = localStorage.getItem("token");

        if (this.state.token == "") {
            window.location.href = '/';
        }

        document.title = "Truck"

    }

    changeDate = (event) => {
        this.setState({ date: event.format("DD-MM-YYYY") })
    }

    handleChangeOrganization = (e) => {
        this.setState({ organization_id: e.target.value });

    }

    componentDidMount() {
        const response2 = api.get("/api/v1/truck", {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

        response2.then(result => this.setState({ trucks: result.data }));

        const response = api.get("/api/v1/truckBreakDown/" + window.location.href.split('/')[4], {
            headers: { Authorization: `Bearer ${this.state.token}` }
        });

         //revision
        response.then(result => this.setState({ description: result.data.description ,price:  result.data.price, olddate: result.data.date}));

    }

 

    handlePost = async e => {
        e.preventDefault();
 
        const { description, price, date } = this.state;
            if(date == ""){
                alert(`Coloque a data !`); 
            }else{
                const response = await api.put("/api/v1/truckBreakDown/" + window.location.href.split('/')[4], JSON.stringify({
                    description: description,
                    price: price,
                    truckId: window.location.href.split('/')[4],
                    date: date,
    
                }), {
                    headers: {
                        Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
    
                if (response.status == 200) {
                    this.setState({ error: "Atualizado com sucesso!", showAlert: true });
    
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
                            <h1 >Editar a Revisão de um Camião</h1>

                        </div>
                        <div className="row g-4">
                            <div className="col-sm-12 col-xl-12">
                                <div className="bg-light rounded h-100 p-4">
                                    <img src="/img/truck-icon.png" style={{ height: 70 }} />
                                    <form>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Descrição</label>
                                            <input type="text" className="form-control" required id="text" value ={this.state.description} onChange={e => this.setState({ description: e.target.value })} />
                                        </div>

                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Preço</label>
                                            <input type="text" className="form-control" required id="text" value ={this.state.price}  onChange={e => this.setState({ price: e.target.value })} />
                                            
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="exampleInputEmail1" className="form-label" >Data Antiga</label>
                                            <input type="text" className="form-control" required id="text" value ={this.state.olddate}  onChange={e => this.setState({ price: e.target.value })} />
                                            
                                        </div>
                                        <label htmlFor="exampleInputEmail1" className="form-label" >Data Nova</label>
                                        <div className="form-floating mb-3">
                                            <Datetime ref="datetime"
                                                onChange={this.changeDate}
                                                timeFormat={false} />
                                        </div>
                                        <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 170 }}>Atualizar Revisão</button>
                                        <h3 class="text-center mb-0">{this.state.error}</h3>

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
            </div>
        )
    }
}


export default UpdateRevision

