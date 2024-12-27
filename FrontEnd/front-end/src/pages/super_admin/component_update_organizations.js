import React from 'react';
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import api from '../../service/api';
import Footer from '../../components/utils/footer';

class UpdateOrganization extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      location: "",
      token: "",
      name: "",
      vatin: -1,
      organization_id: "",
      error: "",
      showAlert: false
    }
    this.state.token = localStorage.getItem("token");

    if (this.state.token == "") {
      window.location.href = '/';
    }
    document.title = "Adicionar Organização"
  }
  //Selects
  handleChangeProfile = (e) => {
    this.setState({ profile: e.target.value });
  }

  handleChangeOrganization = (e) => {
    console.log("Organization ", e.target.value)
    this.setState({ organization_id: e.target.value });
  }

  handlePost = async e => {
    e.preventDefault();
    let isToPost = true
    const { name, location, vatin } = this.state;

    if (name == "") {
      this.setState({ error: "Atribua nome à  Organização!" });
      isToPost = false
    }
    if (vatin == -1) {
      this.setState({ error: "Atribua um numero à Vatin" });
      isToPost = false
    }
    if (location == "") {
      this.setState({ error: "Atribua uma localização à organização" });
      isToPost = false
    }

    if (isToPost) {
      this.setState({ error: "Nome já pertence a uma Organização" });
      const response = await api.put("/api/v1/organization/" + window.location.href.split('/')[4], JSON.stringify({
        name: name,
        enable: true,
        vatin: vatin,
        addresses: location

      }), {
        headers: { Authorization: `Bearer ${this.state.token}`, 'Accept':'application/json',
        'Content-Type':'application/json' }
      });
      
    
      if (response.status == 200) {
        this.setState({ error: "Atualizado com sucesso!" ,location:"", vatin:-1,name:""});

      }
    }
  }
  
  componentDidMount() {
    const response = api.get("/api/v1/organization/" + window.location.href.split('/')[4], {
        headers: { Authorization: `Bearer ${this.state.token}` }
    });

     //revision
    response.then(result => this.setState({ name: result.data.name ,location:  result.data.addresses, vatin : result.data.vatin}));
 
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
              <h1>Editar Organização</h1>
            </div>
            <div className="row g-4">
              <div className="col-sm-12 col-xl-12">
                <div className="bg-light rounded h-100 p-4">
                  <img src="/img/organization.png" style={{ height: 50 }} />
                  <form>
                    <div className="mb-3">
                      <label htmlFor="exampleInputEmail1" className="form-label" >Nome</label>
                      <input type="text" className="form-control" required id="text" value ={this.state.name} onChange={e => this.setState({ name: e.target.value })} />
                    </div>
                    <div className="mb-3">
                      <label htmlFor="exampleInputPassword1" className="form-label" >Localização</label>
                      <input type="text" required className="form-control" id="exampleInputPassword1" value ={this.state.location} onChange={e => this.setState({ location: e.target.value })} />
                    </div> 
                    <div className="mb-3">
                      <label htmlFor="exampleInputPassword1" className="form-label" >Vatin</label>
                      <input type="text" required className="form-control" id="exampleInputPassword1" value ={this.state.vatin} onChange={e => this.setState({ vatin: e.target.value })} />
                    </div>

                    <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 200 }}>Atualizar Organização</button>
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
        {/* Content End */}
      </div>
    )
  }
}


export default UpdateOrganization

