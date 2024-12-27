import React from 'react';
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import api from '../../service/api';
import Footer from '../../components/utils/footer';

class UpdateUser extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      token: "",
      name: "",
      organization_id: "",
      error: "",
      name: "",
      email:""
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
    const { name, email } = this.state;

    if (name == "") {
      this.setState({ error: "Atribua nome à  User!" });
      isToPost = false
    }
    if (email == -1) {
      this.setState({ error: "Atribua um email" });
      isToPost = false
    }
   

    if (isToPost) {
      const response = await api.put("/api/v1/user/" + window.location.href.split('/')[4], JSON.stringify({
        name: name,
        email: email,
        password: "Para_Enganar_o_Required",
        username: "LALALA",
        photofilename :"LALALA",

      }), {
        headers: { Authorization: `Bearer ${this.state.token}`, 'Accept':'application/json',
        'Content-Type':'application/json' }
      });
      
    
      if (response.status == 200) {
        this.setState({ error: "Atualizado com sucesso!" });

      }
    }
  }
  
  componentDidMount() {
    const response = api.get("/api/v1/user/" + window.location.href.split('/')[4], {
        headers: { Authorization: `Bearer ${this.state.token}` }
    });

     //revision
    response.then(result => this.setState({ name: result.data.name ,email:  result.data.email}));
 
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
              <h1>Editar User</h1>
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
                      <label htmlFor="exampleInputPassword1" className="form-label" >Email</label>
                      <input type="text" required className="form-control" id="exampleInputPassword1" value ={this.state.email} onChange={e => this.setState({ email: e.target.value })} />
                    </div> 
                    <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 200 }}>Atualizar User</button>
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


export default UpdateUser

