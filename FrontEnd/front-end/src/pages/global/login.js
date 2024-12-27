import React from 'react';
import api from '../../service/api';
import { Profiles } from '../../global/profile';
class Login extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      username: "",
      password: "",
      error: ""
    }

    document.title = "Login"
  }

  //Login Method
  handleSignIn = async e => {
    const { username, password } = this.state;

    this.setState({ error: "Preencha e-mail e password válidos para entrar!" });

    if(username == ""){
      alert(`Insira dados válidos`);
      window.location.reload(false);
    }

    const response = await api.post("/api/v1/login", {
      username: username,
      password: password
    });

    if (response.data.token != null) {
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("id", response.data.id);
      localStorage.setItem("photopath", response.data.photoPath);
      localStorage.setItem("name", response.data.name)
      // Verify Profile
      if (response.data.role == Profiles.MANAGER.description) {
        window.location.href = '/homemanager';
      } else if(response.data.role == Profiles.SUPER_ADMIN.description) {
        window.location.href = '/home';
      }
    }else{
      alert(`Login Falhou !`);
    }
  };


  render() {
    return (
      <div className="container-xxl position-relative bg-white d-flex p-0">

        <div className="container-fluid">
          <div className="row h-100 align-items-center justify-content-center" style={{ minHeight: '100vh' }}>
            <div className="col-12 col-sm-8 col-md-6 col-lg-5 col-xl-4">
              <div className="bg-light rounded p-4 p-sm-5 my-4 mx-3">
                <div className="d-flex align-items-center justify-content-between mb-3">
                  <a href="index.html" className>
                    <h3 className="text-primary"><i className="fa fa-hashtag me-2" />RETRUCK</h3>
                  </a>
              
                </div>
                <div className="form-floating mb-3">
                  <input type="email" className="form-control" id="floatingInput" placeholder="name@example.com" onChange={e => this.setState({ username: e.target.value })} />
                  <label htmlFor="floatingInput">Username</label>
                </div>
                <div className="form-floating mb-4">
                  <input type="password" className="form-control" id="floatingPassword" placeholder="Password" onChange={e => this.setState({ password: e.target.value })} />
                  <label htmlFor="floatingPassword">Password</label>
                </div>

                <button type="submit" className="btn btn-primary py-3 w-100 mb-4" onClick={this.handleSignIn}>Login</button>
                <p class="text-center mb-0">{this.state.error}</p>
              </div>
            </div>
          </div>
        </div>
        {/* Sign In End */}
      </div>

    )

  }
}


export default Login
